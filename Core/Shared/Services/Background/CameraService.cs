using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.Camera;
using Core.Shared.SignalR.CameraHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

public class CameraService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<CameraService> _logger;
	private string _imagesPath = string.Empty;
	private string _thumbnailsPath = string.Empty;
	private string _extension = string.Empty;
	private IHubContext<CameraHub, ICameraHub> _hubContext = null!;

	public CameraService(IServiceScopeFactory factory, ILogger<CameraService> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	private enum CameraNb
	{
		Camera1 = 1,
		Camera2 = 2,
		Both = 3,
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		_hubContext = asyncScope.ServiceProvider.GetRequiredService<IHubContext<CameraHub, ICameraHub>>();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		int port1 = configuration.GetValueWithThrow<int>("CameraConfig:Camera1:Port");
		int port2 = configuration.GetValueWithThrow<int>("CameraConfig:Camera2:Port");
		_imagesPath = configuration.GetValueWithThrow<string>("CameraConfig:ImagesPath");
		_thumbnailsPath = configuration.GetValueWithThrow<string>("CameraConfig:ThumbnailsPath");
		_extension = configuration.GetValueWithThrow<string>("CameraConfig:Extension");
		if (Station.Type != StationType.S5)
		{
			// Create an instance of the camera
			Task task1 = RunAcquisition(port1, CameraNb.Camera1, stoppingToken);
			Task task2 = RunAcquisition(port2, CameraNb.Camera2, stoppingToken);
			await task1;
			await task2;
		}
		else
		{
			await RunAcquisition(port1, CameraNb.Both, stoppingToken);
		}
	}

	private async Task RunAcquisition(
		int port,
		CameraNb cameraNb,
		CancellationToken cancel)
	{
		Device device = await CameraConnectionManager.Connect(port, cancel);
		device.Notify[NotifyDictionary.DeviceDisconnected].Event += Disconnect;
		device.Notify[NotifyDictionary.DeviceReconnect].Event += Reconnect;

		AdsClient tcClient = new();
		tcClient.Connect(ADSUtils.AdsPort);
		if (!tcClient.IsConnected)
			throw new("Not connected");

		uint ridStructHandle = tcClient.CreateVariableHandle(ADSUtils.GlobalRID);
		uint anodeTypeHandle = tcClient.CreateVariableHandle(ADSUtils.GlobalAnodeType);
		uint pictureCounter
			= tcClient.CreateVariableHandle((cameraNb == CameraNb.Camera2)
				? ADSUtils.PictureCountCam2
				: ADSUtils.PictureCountCam1);

        Stemmer.Cvb.Driver.Stream stream = device.Stream;
		if (!stream.IsRunning)
			stream.Start();

		int nbPictures = 0;
		int nbPicturesTest = 0;

		await Task.Run(
			async () =>
			{
				while (!cancel.IsCancellationRequested)
				{
					try
					{
						if (!device.ConnectionState.HasFlag(ConnectionState.Connected))
						{
							Debug.WriteLine("connection lost");
							Thread.Sleep(1000);
							continue;
						}

						if (!stream.IsRunning)
							stream.Start();

						using StreamImage image = stream.Wait();
						if (await IsTestModeOn(_logger))
						{
							// testDir2 != null means that we are in a S5 Cycle.
							string dir = (cameraNb == CameraNb.Both && nbPicturesTest % 2 == 1)
								? ShootingUtils.CameraTest2
								: ShootingUtils.CameraTest1;
							FileInfo previousImage = new(dir + ShootingUtils.TestFilename);
							if (previousImage.Exists)
								previousImage.Delete();

							image.Save(dir + ShootingUtils.TestFilename, 1);
							++nbPicturesTest;
							await _hubContext.Clients.All.RefreshTestImages();
						}
						else
						{
							RIDStruct rid = tcClient.ReadAny<RIDStruct>(ridStructHandle);
							string anodeType = AnodeTypeDict.AnodeTypeIntToString(tcClient.ReadAny<int>(anodeTypeHandle));
							int cameraID = (cameraNb != CameraNb.Both)
                                ? (int)cameraNb
								: (int)((nbPictures % 2 == 1) ? CameraNb.Camera2 : CameraNb.Camera1);
							FileInfo imagePath
								= Shooting.GetImagePathFromRoot(rid.ToRID(), Station.ID, _imagesPath, anodeType, cameraID, _extension);
							FileInfo thumbnailPath
								= Shooting.GetImagePathFromRoot(rid.ToRID(), Station.ID, _thumbnailsPath, anodeType, cameraID, _extension);

							if (imagePath.DirectoryName is not null)
								Directory.CreateDirectory(imagePath.DirectoryName);

							image.Save(imagePath.FullName, 1);

							if (thumbnailPath.DirectoryName is not null)
								Directory.CreateDirectory(thumbnailPath.DirectoryName);

							image.Save(thumbnailPath.FullName, 0.2);

							image.Close();

							tcClient.WriteAny(pictureCounter, (ushort)1);
							++nbPictures;
						}
					}
					catch (Exception e)
					{
						// Warning -> In prod, this condition is called before the disconnect function
						// If the program crash, put this inside a try/catch or create a global var
						// If the stream is always active, stop it
						_logger.LogError("Error while monitoring camera with port {port}: {e}",port, e);
						if (device.Stream.IsRunning)
							stream.TryStop();

						stream.Start();
					}
				}

				if (device.Stream.IsRunning)
					stream.TryStop();

				return Task.CompletedTask;
			},
			cancel);
	}

	#region Generics functions

	private static async Task<bool> IsTestModeOn(ILogger<CameraService> logger)
	{
		using HttpClient http = new();
		try
		{
			HttpResponseMessage response = await http.GetAsync(
				$"{ITApisDict.IOTAddress}/apiIOT/IOTTag/{nameof(IOTTag.RID)}/{IOTTagRID.TestMode}");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new ApplicationException($"Response status code is: {response.StatusCode.ToString()}");

			ApiResponse? apiResponse
				= JsonSerializer.Deserialize<ApiResponse>(await response.Content.ReadAsStreamAsync());
			if (apiResponse is null)
				throw new ApplicationException("Could not deserialize ApiIOT response");

			if (apiResponse.Result is not JsonElement jsonElement)
				throw new ApplicationException("JSON Exception, ApiResponse from ApiIOT is broken");

			IOTTag[] tags = jsonElement.Deserialize<IOTTag[]>()
				?? throw new InvalidOperationException(
					$"ApiResponse IOTTags are null or invalid: {jsonElement.ToString()}");
			return tags.Length != 0 && bool.Parse(tags[0].CurrentValue);
		}
		catch (Exception e)
		{
			logger.LogError("ApiCamera could not communicate with ApiIOT: {error}", e);
			// In doubt, we are most likely not in test mode.
			return false;
		}
	}

	private static void Disconnect(object? sender, NotifyEventArgs e)
	{
		Console.WriteLine("disconnected");

		if (sender is null)
			return;

		Device? device = (Device?)sender.GetType().GetProperty("Parent")?.GetValue(sender);

		if (device is null)
			return;

		try
		{
			bool isOk = device.Stream.TryStop();
			if (!isOk)
				Debug.WriteLine("Could not stop the stream!");
		}
		catch (Exception ex)
		{
			Debug.WriteLine("Error: " + ex.Message);
		}
	}

	private static void Reconnect(object? sender, NotifyEventArgs e)
	{
		Console.WriteLine("reconnected");

		Device? device = (Device?)sender?.GetType().GetProperty("Parent")?.GetValue(sender);

		if (device is null)
			return;

		try
		{
			device.Stream.Start();
		}
		catch (Exception ex)
		{
			Debug.WriteLine("Error: " + ex.Message);
		}
	}

	#endregion
}
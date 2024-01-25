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

/// <summary>
/// Background service used to "wait" for camera images and save them in the correct folder.
/// </summary>
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
		int port1 = configuration.GetValueWithThrow<int>(ConfigDictionary.Camera1Port);
		int port2 = configuration.GetValueWithThrow<int>(ConfigDictionary.Camera2Port);
		_imagesPath = configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		_thumbnailsPath = configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		_extension = configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
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
							string dir = (GetCameraID(cameraNb, tcClient) == 1) ? ShootingUtils.CameraTest1 : ShootingUtils .CameraTest2;
							Directory.CreateDirectory(dir);
							FileInfo previousImage = new(dir + ShootingUtils.TestFilename);
							if (previousImage.Exists)
								previousImage.Delete();

							image.Save(dir + ShootingUtils.TestFilename, 1);
							image.Close();
							await _hubContext.Clients.All.RefreshTestImages();
						}
						else
						{
							RIDStruct rid = tcClient.ReadAny<RIDStruct>(ridStructHandle);
							string anodeType = AnodeTypeDict.AnodeTypeIntToString(tcClient.ReadAny<int>(anodeTypeHandle));
							int cameraID = GetCameraID(cameraNb, tcClient);
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

	private static int GetCameraID(CameraNb cameraNb, AdsClient tcClient)
	{
		if (cameraNb != CameraNb.Both)
			return (int)cameraNb;

		uint isHole1Handle = tcClient.CreateVariableHandle(ADSUtils.IsHole1);
		return (tcClient.ReadAny<bool>(isHole1Handle)) ? (int)CameraNb.Camera1 : (int)CameraNb.Camera2;
	}

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
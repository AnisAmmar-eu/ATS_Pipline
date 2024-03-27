using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.Packets.Services;
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
using Stemmer.Cvb.Utilities;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for receiving images from both cameras and storing them in the right folder.
/// Wait 5 seconds before starting to avoid any conflict with the Stemmer.Cvb library.
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
		await Task.Delay(20000);
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		_hubContext = asyncScope.ServiceProvider.GetRequiredService<IHubContext<CameraHub, ICameraHub>>();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		IPacketService packetService = asyncScope.ServiceProvider.GetRequiredService<IPacketService>();
		int port1 = configuration.GetValueWithThrow<int>(ConfigDictionary.Camera1Port);
		int port2 = configuration.GetValueWithThrow<int>(ConfigDictionary.Camera2Port);
		double timeOutCamera = configuration.GetValueWithThrow<int>(ConfigDictionary.TimeOutCamera);
		_imagesPath = configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		_thumbnailsPath = configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		_extension = configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		if (Station.Type != StationType.S5)
		{
			// Create an instance of the camera
			Task task1 = RunAcquisition(port1, CameraNb.Camera1, timeOutCamera, packetService ,stoppingToken);
			Task task2 = RunAcquisition(port2, CameraNb.Camera2, timeOutCamera, packetService ,stoppingToken);
			await task1;
			await task2;
		}
		else
		{
			await RunAcquisition(port1, CameraNb.Both, timeOutCamera, packetService ,stoppingToken);
		}
	}

	private async Task RunAcquisition(
		int port,
		CameraNb cameraNb,
		double timeOutCamera,
		IPacketService packetService,
		CancellationToken cancel)
	{
		Device device = await CameraConnectionManager.Connect(port, cancel);
		device.Notify[NotifyDictionary.DeviceDisconnected].Event += Disconnect;
		device.Notify[NotifyDictionary.DeviceReconnect].Event += Reconnect;

		AdsClient tcClient = new();
		tcClient.Connect(ADSUtils.AdsPort);
		if (!tcClient.IsConnected)
			throw new("Not connected");

		Stemmer.Cvb.Driver.Stream stream = device.Stream;
		if (!stream.IsRunning)
			stream.Start();

		WaitStatus status = WaitStatus.Ok;

		await Task.Run(
			async () =>
			{
				while (true)
				{
					try
					{
						if (!device.ConnectionState.HasFlag(ConnectionState.Connected))
						{
							_logger.LogError("connection lost");
							Thread.Sleep(1000);
							continue;
						}

						if (!stream.IsRunning)
							stream.Start();

						using StreamImage image = stream.WaitFor(UsTimeSpan.FromMilliseconds(timeOutCamera), out status);
						if (status != WaitStatus.Ok)
							throw new($"Exception: {status.ToString()}");

						DateTimeOffset ShootingDate = DateTimeOffset.Now;
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
							uint ridStructHandle = tcClient.CreateVariableHandle(ADSUtils.GlobalRID);
							RIDStruct rid = tcClient.ReadAny<RIDStruct>(ridStructHandle);
							uint anodeTypeHandle = tcClient.CreateVariableHandle(ADSUtils.GlobalAnodeType);
							string anodeType = AnodeTypeDict.AnodeTypeIntToString(tcClient.ReadAny<int>(anodeTypeHandle));
							_logger.LogInformation("AnodeType: {anodeType}", anodeType);
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

							Shooting shooting = new()
							{
								StationCycleRID = rid.ToRID(),
								AnodeType = anodeType,
								ShootingTS = ShootingDate,
								Cam01Status = (cameraID == (int)CameraNb.Camera1) ? 1 : 0,
								Cam02Status = (cameraID == (int)CameraNb.Camera2) ? 1 : 0,
								HasError = false,
							};

							await packetService.BuildPacket(shooting);

							await _hubContext.Clients.All.RefreshTestImages();
						}
					}
					catch (Exception e)
					{
						_logger.LogError("WaitStatus: {status}", status);
						_logger.LogError("Error while monitoring camera with port {port}: {e}",port, e);
					}
				}
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

	private void Disconnect(object? sender, NotifyEventArgs e)
	{
	}

	private void Reconnect(object? sender, NotifyEventArgs e)
	{
		_logger.LogError("Reconnecting camera");
		try
		{
			using System.Diagnostics.Process process = new();
			process.StartInfo = new() {
				WindowStyle = ProcessWindowStyle.Normal,
				FileName = "cmd.exe",
				Arguments = "/C %SYSTEMROOT%\\System32\\inetsrv\\appcmd recycle apppool /apppool.name:\"ApiCameraLocal\"",
			};
			process.Start();
			_logger.LogError("Recycling the app pool");
		}
		catch (Exception ex)
		{
			_logger.LogError("Error while recycling the app pool: {ex}", ex.Message);
		}

		//Device? device = (Device?)sender?.GetType().GetProperty("Parent")?.GetValue(sender);

		//if (device is null)
		//	return;

		//try
		//{
		//	bool isOk = device.Stream.TryStop();
		//	if (isOk)
		//		device.Stream.Start();
		//	else
		//		_logger.LogError("Could not stop the stream!");
		//}
		//catch (Exception ex)
		//{
		//	_logger.LogError("Error: " + ex.Message);
		//}
	}

	#endregion
}
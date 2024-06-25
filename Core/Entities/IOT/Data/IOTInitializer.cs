using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Mapster;

namespace Core.Entities.IOT.Data;

public static class IOTInitializer
{
	public static void InitializeStation(AnodeCTX anodeCTX)
	{
		if (anodeCTX.IOTDevice.Any())
			return;

		// Cameras
		InitializeCamera(anodeCTX, DeviceRID.Camera1, "First", 1);
		InitializeCamera(anodeCTX, DeviceRID.Camera2, "Second", 2);

		var apis = new[]
		{
			new { Rid = ITApisDict.ADSRID, Address = ITApisDict.ADSAddress, Path = ITApisDict.ADSPath },
			new { Rid = ITApisDict.AlarmRID, Address = ITApisDict.AlarmAddress, Path = ITApisDict.AlarmPath },
			new { Rid = ITApisDict.CameraRID, Address = ITApisDict.CameraAddress, Path = ITApisDict.CameraPath },
			new { Rid = ITApisDict.IOTRID, Address = ITApisDict.IOTAddress, Path = ITApisDict.IOTPath },
			new { Rid = ITApisDict.StationCycleRID, Address = ITApisDict.StationCycleAddress,
					Path = ITApisDict.StationCyclePath, },
			new { Rid = ITApisDict.ServerReceiveRID, Address = ITApisDict.ServerReceiveAddress,
					Path = ITApisDict.ServerReceivePath, },
		};

		foreach (var api in apis)
			InitializeApi(anodeCTX, api.Rid, api.Address, api.Path);

		InitializeTwinCat(anodeCTX, DeviceRID.TwinCat, ADSUtils.AdsPort.ToString(), ADSUtils.ConnectionPath);
	}

	public static void InitializeServer(AnodeCTX anodeCTX)
	{
		if (anodeCTX.IOTDevice.Any())
			return;

		var serverApis = new[]
			{
				new { Rid = ITApisDict.IOTRID, Address = ITApisDict.IOTAddress, Path = ITApisDict.IOTPath },
				new { Rid = ITApisDict.AlarmRID, Address = ITApisDict.AlarmAddress, Path = ITApisDict.AlarmPath },
				new { Rid = ITApisDict.MonitorRID, Address = ITApisDict.MonitorAddress, Path = ITApisDict.MonitorPath },
				new { Rid = ITApisDict.ServerReceiveRID,
					Address = ITApisDict.ServerReceiveAddress, Path = ITApisDict.ServerReceivePath, },
				new { Rid = ITApisDict.StationCycleRID,
					Address = ITApisDict.StationCycleAddress, Path = ITApisDict.StationCyclePath, },
				new { Rid = ITApisDict.UserRID, Address = ITApisDict.UserAddress, Path = ITApisDict.UserPath },
				new { Rid = ITApisDict.VisionRID, Address = ITApisDict.VisionAddress, Path = ITApisDict.VisionPath },
		};

		foreach (var api in serverApis)
			InitializeApi(anodeCTX, api.Rid, api.Address, api.Path);

		var stationApis = new[]
		{
			new { Rid = ITStationApisDict.Station1RID, Address = ITStationApisDict.Station1Address, Path = ITStationApisDict
				.StationPath, },
			new { Rid = ITStationApisDict.Station2RID, Address = ITStationApisDict.Station2Address, Path = ITStationApisDict
				.StationPath, },
			new { Rid = ITStationApisDict.Station3RID, Address = ITStationApisDict.Station3Address, Path = ITStationApisDict
				.StationPath, },
			new { Rid = ITStationApisDict.Station4RID, Address = ITStationApisDict.Station4Address, Path = ITStationApisDict
				.StationPath, },
			new { Rid = ITStationApisDict.Station5RID, Address = ITStationApisDict.Station5Address, Path = ITStationApisDict
				.StationPath, },
		};

		foreach (var api in stationApis)
			InitializeApiStation(anodeCTX, api.Rid, api.Address, api.Path);

		InitializeServerRule(anodeCTX);
		InitializeSignServices(anodeCTX);
		InitializeMatchServices(anodeCTX);

		if (!anodeCTX.DebugModes.Any())
			InitializeDebugModes(anodeCTX);
	}

	private static void InitializeCamera(AnodeCTX anodeCTX, string rid, string prefix, int port)
	{
		OTCamera cam = new() {
			RID = rid,
			Name = prefix + " Camera",
			Description = prefix + " Camera IOTDevice",
			Address = (port - 1).ToString(),
			IsConnected = false,
		};
		anodeCTX.OTCamera.Add(cam);
		anodeCTX.SaveChanges();
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.TriggerMode + port,
			Name = "Trigger mode",
			Description = "Trigger mode for Camera" + port,
			CurrentValue = "On",
			NewValue = "On",
			HasNewValue = true,
			Path = IOTTagPath.TriggerMode,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.TriggerSource + port,
			Name = "Trigger source",
			Description = "Trigger source for Camera" + port,
			CurrentValue = TriggerSources.Line3,
			NewValue = TriggerSources.Line3,
			HasNewValue = true,
			Path = IOTTagPath.TriggerSource,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.TriggerActivation + port,
			Name = "Trigger activation",
			Description = "Trigger activation for Camera" + port,
			CurrentValue = TriggerActivations.RisingEdge,
			NewValue = TriggerActivations.RisingEdge,
			HasNewValue = true,
			Path = IOTTagPath.TriggerActivation,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.ExposureTime + port,
			Name = "Exposure time",
			Description = "Exposure time for Camera" + port,
			CurrentValue = "30000.0",
			NewValue = "30000.0",
			HasNewValue = true,
			Path = IOTTagPath.ExposureTime,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.AcquisitionFrameRateEnable + port,
			Name = "Acquisition frame rate enable",
			Description = "Acquisition frame rate enable for Camera" + port,
			CurrentValue = "Off",
			NewValue = "Off",
			HasNewValue = false,
			Path = IOTTagPath.AcquisitionFrameRateEnable,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.Gain + port,
			Name = "Gain",
			Description = "Gain for Camera" + port,
			CurrentValue = "10",
			NewValue = "10",
			HasNewValue = true,
			Path = IOTTagPath.Gain,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.BlackLevel + port,
			Name = "Black level",
			Description = "Black level for Camera" + port,
			CurrentValue = "50",
			NewValue = "50",
			HasNewValue = true,
			Path = IOTTagPath.BlackLevel,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.Gamma + port,
			Name = "Gamma",
			Description = "Gamma for Camera" + port,
			CurrentValue = "1",
			NewValue = "1",
			HasNewValue = true,
			Path = IOTTagPath.Gamma,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.BalanceRatio + port,
			Name = "Balance ratio",
			Description = "Balance ratio for Camera" + port,
			CurrentValue = "2.35498",
			NewValue = "2.35498",
			HasNewValue = true,
			Path = IOTTagPath.BalanceRatio,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.ConvolutionMode + port,
			Name = "Convolution mode",
			Description = "Convolution mode for Camera" + port,
			CurrentValue = "Off",
			NewValue = "Off",
			HasNewValue = false,
			Path = IOTTagPath.ConvolutionMode,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.AdaptiveNoiseSuppressionFactor + port,
			Name = "Adaptive noise suppression factor",
			Description = "Adaptive noise suppression factor for Camera" + port,
			CurrentValue = "1",
			NewValue = "1",
			HasNewValue = false,
			Path = IOTTagPath.AdaptiveNoiseSuppressionFactor,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.Sharpness + port,
			Name = "Sharpness",
			Description = "Sharpness for Camera" + port,
			CurrentValue = "0",
			NewValue = "0",
			HasNewValue = false,
			Path = IOTTagPath.Sharpness,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});
		anodeCTX.IOTTag.Add(new IOTTag {
			RID = IOTTagRID.AcquisitionFrameRate + port,
			Name = "Acquisition frame rate",
			Description = "Acquisition frame rate for Camera" + port,
			CurrentValue = "23.9798",
			NewValue = "23.9798",
			HasNewValue = false,
			Path = IOTTagPath.AcquisitionFrameRate,
			IOTDeviceID = cam.ID,
			IOTDevice = cam,
		});

		anodeCTX.SaveChanges();
	}

	private static void InitializeApi(
		AnodeCTX anodeCTX,
		string rid,
		string address,
		string path)
	{
		ITApi api = new() {
			RID = rid,
			Name = rid,
			Description = rid,
			Address = address,
			ConnectionPath = path,
			IsConnected = false,
		};
		anodeCTX.ITApi.Add(api);
		anodeCTX.SaveChanges();
	}

	private static void InitializeApiStation(
		AnodeCTX anodeCTX,
		string rid,
		string address,
		string path)
	{
		ITApiStation api = new() {
			RID = rid,
			Name = rid,
			Description = rid,
			Address = address,
			ConnectionPath = path,
			IsConnected = true,
			OldestTSShooting = DateTimeOffset.MinValue,
			IsOptional = true,
		};
		anodeCTX.ITApiStations.Add(api);
		anodeCTX.SaveChanges();
	}

	private static void InitializeServerRule(
		AnodeCTX anodeCTX)
	{
		anodeCTX.ServerRule.Add(new ServerRule {
			RID = ServerRuleDict.RID,
			Name = ServerRuleDict.RID,
			Description = ServerRuleDict.RID,
			Address = string.Empty,
			ConnectionPath = string.Empty,
			IsConnected = true,
			Reinit = false,
		});

		anodeCTX.SaveChanges();
	}

	private static void InitializeSignServices(AnodeCTX anodeCTX)
	{
		foreach (SignServiceData signService in SignService.list)
		{
			Sign sign = signService.Adapt<Sign>();
			sign.Name = sign.RID;
			sign.Description = sign.RID;
			anodeCTX.Sign.Add(sign);
		}

		anodeCTX.SaveChanges();
	}

	private static void InitializeMatchServices(AnodeCTX anodeCTX)
	{
		foreach (MatchServiceData matchService in MatchService.list)
		{
			Match match = matchService.Adapt<Match>();
			match.Name = match.RID;
			match.Description = match.RID;
			anodeCTX.Match.Add(match);
		}

		anodeCTX.SaveChanges();
	}

	private static void InitializeDebugModes(AnodeCTX anodeCTX)
	{
		anodeCTX.DebugModes.Add(new DebugMode {
			DebugModeEnabled = false,
			LogEnabled = false,
			CsvExportEnabled = false,
			LogSeverity = "Warning",
		});

		anodeCTX.SaveChanges();
	}

	private static void InitializeTwinCat(AnodeCTX anodeCTX, string rid, string address, string path)
	{
		OTTwinCat twinCat = new() {
			RID = rid,
			Name = rid,
			Description = rid,
			Address = address,
			ConnectionPath = path,
			IsConnected = false,
		};
		anodeCTX.OTTwinCat.Add(twinCat);
		anodeCTX.SaveChanges();

		#region Camera Temperature

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TemperatureCam1,
			Name = "Cam 01 - Temperature",
			Description = "Temperature for Camera 1",
			CurrentValue = "19.84",
			NewValue = string.Empty,
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.TemperatureCam1Read,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TemperatureCam2,
			Name = "Cam 02 - Temperature",
			Description = "Temperature for Camera 2",
			CurrentValue = "19.84",
			NewValue = string.Empty,
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.TemperatureCam2Read,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TemperatureStatusCam1,
			Name = "Cam1 temperature status",
			Description = "Cam1 temperature status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.TemperatureStatusCam1,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TemperatureStatusCam2,
			Name = "Cam2 temperature status",
			Description = "Cam2 temperature status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.TemperatureStatusCam2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion Camera Temperature

		#region Status

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TSH01,
			Name = "High temperature status",
			Description = "High temperature status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.TSH01,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.PowerFailure,
			Name = "Power failure status",
			Description = "Power failure status",
			CurrentValue = "false",
			NewValue = string.Empty,
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.PowerFailure,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.AirPressure,
			Name = "Air pressure status",
			Description = "Air pressure status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.AirPressure,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TT01,
			Name = "TT01 status",
			Description = "TT01 status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.TT01,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		if (Station.Type == StationType.S5)
		{
			anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
				RID = IOTTagRID.TT02,
				Name = "TT02 status",
				Description = "TT02 status",
				CurrentValue = "1",
				NewValue = string.Empty,
				ValueType = IOTTagType.UShort,
				HasNewValue = false,
				IsReadOnly = true,
				Path = IOTTagPath.TT02,
				IOTDeviceID = twinCat.ID,
				IOTDevice = twinCat,
			});
		}

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DiagCam1LedOn,
			Name = "Cam1 LedOn status",
			Description = "Cam1 LedOn status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.DiagCam1LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DiagCam1LedOff,
			Name = "Cam1 LedOff status",
			Description = "Cam1 LedOff status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.DiagCam1LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DiagCam2LedOn,
			Name = "Cam2 LedOn status",
			Description = "Cam2 LedOn status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.DiagCam2LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DiagCam2LedOff,
			Name = "Cam2 LedOff status",
			Description = "Cam2 LedOff status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.DiagCam2LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Cam1Status,
			Name = "Cam1 status",
			Description = "Cam1 status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.Cam1StatusRead,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Cam2Status,
			Name = "Cam2 status",
			Description = "Cam2 status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.Cam2StatusRead,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion Status

		#region TestMode

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TestMode,
			Name = IOTTagRID.TestMode,
			Description = "Test mode tag. Should be the sole one.",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.TestMode,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.MsgInit,
			Name = IOTTagRID.MsgInit,
			Description = "MsgInit reset sequence",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.MsgInit,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.SaveChanges();

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Shoot1,
			Name = "Cam 01 - Shoot",
			Description = "Shoot instruction for camera 1",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Shoot1,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Shoot2,
			Name = "Cam 02 - Shoot",
			Description = "Shoot instruction for camera 2",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Shoot2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Led1,
			Name = "LED LFN01",
			Description = "LED LFN01",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Led1,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Led2,
			Name = "LED LFN02",
			Description = "LED LFN02",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Led2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Led3,
			Name = "LED LFN03",
			Description = "LED LFN03",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Led3,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Led4,
			Name = "LED LFN04",
			Description = "LED LFN04",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Led4,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Blowing1,
			Name = "Blowing FV01",
			Description = "Blowing FV01",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Blowing1,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Blowing2,
			Name = "Blowing FV02",
			Description = "Blowing FV02",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Blowing2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.Blowing3,
			Name = "Blowing FV03",
			Description = "Blowing FV03",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.Blowing3,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequencePicture1,
			Name = "Sequence Picture 1",
			Description = "Sequence Picture 1",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequencePicture1,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequencePicture2,
			Name = "Sequence Picture 2",
			Description = "Sequence Picture 2",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequencePicture2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCleaningCam,
			Name = "Sequence Cleaning Cam1",
			Description = "Sequence Cleaning Cam1",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCleaningCam,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCoolingCam,
			Name = "Sequence Cooling Cam",
			Description = "Sequence Cooling Cam",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCoolingCam,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCleanLed,
			Name = "Sequence Cooling Led On",
			Description = "Sequence Cooling Led On",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCleanLed,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCam1LedOn,
			Name = "Sequence Cam1 Led On",
			Description = "Sequence Cam1 Led On",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCam1LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCam1LedOff,
			Name = "Sequence Cam1 Led Off",
			Description = "Sequence Cam1 Led Off",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCam1LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCam2LedOn,
			Name = "Sequence Cam2 Led On",
			Description = "Sequence Cam2 Led On",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCam2LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SequenceCam2LedOff,
			Name = "Sequence Cam2 Led Off",
			Description = "Sequence Cam2 Led Off",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCam2LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion TestMode

		#region Shooting

		#region D20

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFlashD20,
			Name = "Delay Flash D20",
			Description = "Delay Flash D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayFlashD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationFlashD20,
			Name = "Duration Flash D20",
			Description = "Duration Flash D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationFlashD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCamD20,
			Name = "Delay Cam D20",
			Description = "Delay Cam D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayCamD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HightTopD20,
			Name = "Height Top D20",
			Description = "Height Top D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HightTopD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HightMinD20,
			Name = "Height Min D20",
			Description = "Height Min D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HightMinD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HoleMaxD20,
			Name = "Hole Max D20",
			Description = "Hole Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HoleMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HoleMinD20,
			Name = "Hole Min D20",
			Description = "Hole Min D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HoleMinD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion D20

		#region DX

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFlashDX,
			Name = "Delay Flash DX",
			Description = "Delay Flash DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayFlashDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationFlashDX,
			Name = "Duration Flash DX",
			Description = "Duration Flash DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationFlashDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCamDX,
			Name = "Delay Cam DX",
			Description = "Delay Cam DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayCamDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HightTopDX,
			Name = "Height Top DX",
			Description = "Height Top DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HightTopDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HightMinDX,
			Name = "Height Min DX",
			Description = "Height Min DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HightMinDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HoleMaxDX,
			Name = "Hole Max DX",
			Description = "Hole Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HoleMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HoleMinDX,
			Name = "Hole Min DX",
			Description = "Hole Min DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HoleMinDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion DX

		#endregion Shooting

		#region Announcement

		// Cleaning and Temperature Fields
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CleanTopDetectDelay,
			Name = "Clean Top Detect Delay",
			Description = "Delay for clean top detection",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.CleanTopDetectDelay,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CleanTopDelay,
			Name = "Clean Top Delay",
			Description = "Delay before clean top starts",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.CleanTopDelay,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CleanTopDuration,
			Name = "Clean Top Duration",
			Description = "Duration of the clean top process",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.CleanTopDuration,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.SyncEmptyStackDelay,
			Name = "Sync Empty Stack Delay",
			Description = "Waiting Delay before taking into account MSG from PLC RW - seconds",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.SyncEmptyStackDelay,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion Announcement

		#region Anode

		#region D20

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMaxD20,
			Name = "Length Max D20",
			Description = "Length Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.LengthMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMinD20,
			Name = "Length Min D20",
			Description = "Length Min D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.LengthMinD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMaxD20,
			Name = "Width Max D20",
			Description = "Width Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.WidthMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMinD20,
			Name = "Width Min D20",
			Description = "Width Min D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.WidthMinD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.InHoleDelayD20,
			Name = "In Hole Delay D20",
			Description = "In Hole Delay D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.InHoleDelayD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthDelayMaxD20,
			Name = "Width Delay Max D20",
			Description = "Width Delay Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.WidthDelayMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthDelayMaxD20,
			Name = "Length Delay Max D20",
			Description = "Length Delay Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.LengthDelayMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion D20

		#region DX

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMaxDX,
			Name = "Length Max DX",
			Description = "Length Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.LengthMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMinDX,
			Name = "Length Min DX",
			Description = "Length Min DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.LengthMinDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMaxDX,
			Name = "Width Max DX",
			Description = "Width Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.WidthMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMinDX,
			Name = "Width Min DX",
			Description = "Width Min DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.WidthMinDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.InHoleDelayDX,
			Name = "In Hole Delay DX",
			Description = "In Hole Delay DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.InHoleDelayDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthDelayMaxDX,
			Name = "Width Delay Max DX",
			Description = "Width Delay Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.WidthDelayMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthDelayMaxDX,
			Name = "Length Delay Max DX",
			Description = "Length Delay Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.LengthDelayMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion DX

		#endregion Anode

		#region Health

		// Camera Cooling Fields
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CameraCoolingPeriod,
			Name = "FV01 - Camera Cooling Period",
			Description = "Period of the camera cooling cycle",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.CameraCoolingPeriod,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CameraCoolingFrequencyNormal,
			Name = "FV01 - Camera Cooling Frequency Normal",
			Description = "Normal frequency for camera cooling",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.CameraCoolingFrequencyNormal,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CameraCoolingFrequencyHot,
			Name = "FV01 - Camera Cooling Frequency Hot",
			Description = "Frequency for camera cooling when hot",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.CameraCoolingFrequencyHot,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CameraCoolingTempWarning,
			Name = "FV01 - Camera Cooling Temp Warning",
			Description = "Warning temperature for camera cooling",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.CameraCoolingTempWarning,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CameraCoolingDuration,
			Name = "FV01 - Camera Cooling Duration",
			Description = "Duration of the camera cooling",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.CameraCoolingDuration,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		// LED Blowing Fields
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LedBlowingPeriod,
			Name = "FV02 - LED Blowing Period",
			Description = "Period of the LED blowing cycle",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.LedBlowingPeriod,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LedBlowingFrequencyNormal,
			Name = "FV02 - LED Blowing Frequency Normal",
			Description = "Normal frequency for LED blowing",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LedBlowingFrequencyNormal,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LedBlowingFrequencyHot,
			Name = "FV02 - LED Blowing Frequency Hot",
			Description = "Frequency for LED blowing when hot",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LedBlowingFrequencyHot,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LedBlowingTempWarning,
			Name = "FV02 - LED Blowing Temp Warning",
			Description = "Warning temperature for LED blowing",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.LedBlowingTempWarning,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LedBlowingDuration,
			Name = "FV02 - LED Blowing Duration",
			Description = "Duration of the LED blowing",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.LedBlowingDuration,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HotAnodeTT01,
			Name = "Anode Ambient TT01",
			Description = "Temperature set point for hot anode ambient",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HotAnodeTT01,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HotAnodeTT02,
			Name = "Anode Surface TT02",
			Description = "Temperature set point for hot anode surface",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.HotAnodeTT02,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WarnCam01Temp,
			Name = "Alert CAM01 Temp",
			Description = "Warning temperature for CAM01",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.WarnCam01Temp,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WarnCam02Temp,
			Name = "Alert CAM02 Temp",
			Description = "Warning temperature for CAM02",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.WarnCam02Temp,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CamRestartDelay,
			Name = "Cam Restart Delay",
			Description = "Delay before restarting the camera after health shutdown",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.CamRestartDelay,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion Health

		#region Diagnostic

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LuminosityCheckFrequencyCam1LedOn,
			Name = "Cam1 Led On Luminosity Check Frequency",
			Description = "Cam1 Led On Luminosity Check Frequency",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LuminosityCheckFrequencyCam1LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ThresholdLuminosityCam1LedOn,
			Name = "Cam1 Led On Threshold Luminosity",
			Description = "Cam1 Led On Threshold Luminosity",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ThresholdLuminosityCam1LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCheckCam1LedOn,
			Name = "Cam1 Led On Delay Check",
			Description = "Cam1 Led On Delay Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayCheckCam1LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationLuxCheckCam1LedOn,
			Name = "Cam1 Led On Duration Lux Check",
			Description = "Cam1 Led On Duration Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationLuxCheckCam1LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ThresholdLuminosityCam1LedOff,
			Name = "Cam1 Led Off Threshold Luminosity",
			Description = "Cam1 Led Off Threshold Luminosity",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ThresholdLuminosityCam1LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LuminosityCheckFrequencyCam1LedOff,
			Name = "Cam1 Led Off Luminosity Check Frequency",
			Description = "Cam1 Led Off Luminosity Check Frequency",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LuminosityCheckFrequencyCam1LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCheckCam1LedOff,
			Name = "Cam1 Led Off Delay Check",
			Description = "Cam1 Led Off Delay Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayCheckCam1LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationLuxCheckCam1LedOff,
			Name = "Cam1 Led Off Duration Lux Check",
			Description = "Cam1 Led Off Duration Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationLuxCheckCam1LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LuminosityCheckFrequencyCam2LedOn,
			Name = "Cam2 Led On Luminosity Check Frequency",
			Description = "Cam2 Led On Luminosity Check Frequency",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LuminosityCheckFrequencyCam2LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ThresholdLuminosityCam2LedOn,
			Name = "Cam2 Led On Threshold Luminosity",
			Description = "Cam2 Led On Threshold Luminosity",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ThresholdLuminosityCam2LedOn,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationLuxCheckCam2LedOn,
			Name = "Cam2 Led On Duration Lux Check",
			Description = "Cam2 Led On Duration Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationLuxCheckCam2LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationLuxCheckCam2LedOn,
			Name = "Cam2 Led On Duration Lux Check",
			Description = "Cam2 Led On Duration Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationLuxCheckCam2LedOn,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ThresholdLuminosityCam2LedOff,
			Name = "Cam2 Led Off Threshold Luminosity",
			Description = "Cam2 Led Off Threshold Luminosity",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ThresholdLuminosityCam2LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LuminosityCheckFrequencyCam2LedOff,
			Name = "Cam2 Led Off Luminosity Check Frequency",
			Description = "Cam2 Led Off Luminosity Check Frequency",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LuminosityCheckFrequencyCam2LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCheckCam2LedOff,
			Name = "Cam2 Led Off Delay Check",
			Description = "Cam2 Led Off Delay Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DelayCheckCam2LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationLuxCheckCam2LedOff,
			Name = "Cam2 Led Off Duration Lux Check",
			Description = "Cam2 Led Off Duration Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			Path = IOTTagPath.DurationLuxCheckCam2LedOff,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion Diagnostic

		#region Lasers

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ZT1,
			Name = "Laser ZT1",
			Description = "Laser ZT1",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ZT1,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ZT2,
			Name = "Laser ZT2",
			Description = "Laser ZT2",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ZT2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ZT3,
			Name = "Laser ZT3",
			Description = "Laser ZT3",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ZT3,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ZT4,
			Name = "Laser ZT4",
			Description = "Laser ZT4",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.ZT4,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		// AT01 AT02
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.AT01,
			Name = "Luminosity AT01",
			Description = "Luminosity AT01",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.AT01,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.AT02,
			Name = "Luminosity AT02",
			Description = "Luminosity AT02",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.Float,
			HasNewValue = false,
			Path = IOTTagPath.AT02,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion Lasers

		anodeCTX.SaveChanges();
	}
}
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Data;
using Core.Shared.Dictionaries;

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

		// APIs
		string[] rids = [
			ITApisDict.ADSRID, ITApisDict.AlarmRID, ITApisDict.CameraRID, ITApisDict.IOTRID, ITApisDict.StationCycleRID,
		];
		string[] addresses = [
			ITApisDict.ADSAddress, ITApisDict.AlarmAddress, ITApisDict.CameraAddress, ITApisDict.IOTAddress, ITApisDict
				.StationCycleAddress,
		];
		string[] paths = [
			ITApisDict.ADSPath, ITApisDict.AlarmPath, ITApisDict.CameraPath, ITApisDict.IOTPath, ITApisDict.StationCyclePath,
		];
		for (int i = 0; i < rids.Length; ++i)
			InitializeApi(anodeCTX, rids[i], addresses[i], paths[i]);

		// TODO Path.
		InitializeTwinCat(anodeCTX, DeviceRID.TwinCat, ADSUtils.AdsPort.ToString(), ADSUtils.ConnectionPath);
	}

	public static void InitializeServer(AnodeCTX anodeCTX)
	{
		if (anodeCTX.IOTDevice.Any())
			return;

		// No need for test mode in server.
		string[] rids = [
			ITApisDict.IOTRID, ITApisDict.AlarmRID, ITApisDict.KPIRID, ITApisDict.MonitorRID, ITApisDict.ServerReceiveRID,
			ITApisDict.StationCycleRID, ITApisDict.UserRID, ITApisDict.VisionRID,
		];
		string[] addresses = [
			ITApisDict.IOTAddress, ITApisDict.AlarmAddress, ITApisDict.KPIAddress, ITApisDict.MonitorAddress,
			ITApisDict.ServerReceiveAddress, ITApisDict.StationCycleAddress, ITApisDict.UserAddress,
			ITApisDict.VisionAddress,
		];
		string[] paths = [
			ITApisDict.IOTPath, ITApisDict.AlarmPath, ITApisDict.KPIPath, ITApisDict.MonitorPath,
			ITApisDict.ServerReceivePath, ITApisDict.StationCyclePath, ITApisDict.UserPath, ITApisDict.VisionPath,
		];
		for (int i = 0; i < rids.Length; ++i)
			InitializeApi(anodeCTX, rids[i], addresses[i], paths[i]);
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
		//anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
		//	RID = IOTTagRID.TemperatureOkWarnThreshold,
		//	Name = "Ok - Warn Threshold",
		//	Description = "Ok - Warn temperature threshold",
		//	CurrentValue = "42.0",
		//	NewValue = string.Empty,
		//	ValueType = IOTTagType.Float,
		//	HasNewValue = false,
		//	IsReadOnly = true,
		//	Path = IOTTagPath.TemperatureOkWarnThreshold,
		//	IOTDeviceID = twinCat.ID,
		//	IOTDevice = twinCat,
		//});
		//anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
		//	RID = IOTTagRID.TemperatureWarnErrorThreshold,
		//	Name = "Warn - Error Threshold",
		//	Description = "Warn - Error temperature threshold",
		//	CurrentValue = "70.0",
		//	NewValue = string.Empty,
		//	ValueType = IOTTagType.Float,
		//	HasNewValue = false,
		//	IsReadOnly = true,
		//	Path = IOTTagPath.TemperatureWarnErrorThreshold,
		//	IOTDeviceID = twinCat.ID,
		//	IOTDevice = twinCat,
		//});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TemperatureStatusCam1,
			Name = "Cam1 temperature status",
			Description = "Cam1 temperature status",
			CurrentValue = "1",
			NewValue = string.Empty,
			ValueType = IOTTagType.Int,
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
			ValueType = IOTTagType.Int,
			HasNewValue = false,
			IsReadOnly = true,
			Path = IOTTagPath.TemperatureStatusCam2,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion

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

		#endregion

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
			RID = IOTTagRID.SequenceCoolingLed,
			Name = "Sequence Cooling Led",
			Description = "Sequence Cooling Led",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			Path = IOTTagPath.SequenceCoolingLed,
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

		#endregion

		#region Shooting

		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.RetentiveShootingWaitTimer,
			Name = "Retentive Shooting Wait Timer",
			Description = "Retentive Shooting Wait Timer",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.RetentiveShootingWaitTimer,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFlashD20,
			Name = "Delay Flash D20",
			Description = "Delay Flash D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayFlashD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFlashDX,
			Name = "Delay Flash DX",
			Description = "Delay Flash DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayFlashDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFlashInvalid,
			Name = "Delay Flash Invalid",
			Description = "Delay Flash Invalid",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayFlashInvalid,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationFlashD20,
			Name = "Duration Flash D20",
			Description = "Duration Flash D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
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
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayCamD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCamDX,
			Name = "Delay Cam DX",
			Description = "Delay Cam DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayCamDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayCamInvalid,
			Name = "Delay Cam Invalid",
			Description = "Delay Cam Invalid",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayCamInvalid,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TriggerThreshold1DX,
			Name = "Trigger Threshold1 DX",
			Description = "Trigger Threshold1 DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TriggerThreshold1DX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TriggerThreshold1D20,
			Name = "Trigger Threshold1 D20",
			Description = "Trigger Threshold1 D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TriggerThreshold1D20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TriggerThreshold2DX,
			Name = "Trigger Threshold2 DX",
			Description = "Trigger Threshold2 DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TriggerThreshold2DX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TriggerThreshold2D20,
			Name = "Trigger Threshold2 D20",
			Description = "Trigger Threshold3 D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TriggerThreshold2D20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TriggerThreshold3DX,
			Name = "Trigger Threshold3 DX",
			Description = "Trigger Threshold3 DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TriggerThreshold3DX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TriggerThreshold3D20,
			Name = "Trigger Threshold3 D20",
			Description = "Trigger Threshold3 D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TriggerThreshold3D20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayValidLaser,
			Name = "Delay Valid Laser",
			Description = "Delay Valid Laser",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayValidLaser,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.TransferTimer,
			Name = "Transfer Timer",
			Description = "Transfer Timer",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.TransferTimer,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/

		#endregion

		#region Anode

		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMinD20,
			Name = "Length Min D20",
			Description = "Length Min D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LengthMinD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMaxD20,
			Name = "Length Max D20",
			Description = "Length Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LengthMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMinDX,
			Name = "Length Min DX",
			Description = "Length Min DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LengthMinDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthMaxDX,
			Name = "Length Max DX",
			Description = "Length Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LengthMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMinD20,
			Name = "Width Min D20",
			Description = "Width Min D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.WidthMinD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMaxD20,
			Name = "Width Max D20",
			Description = "Width Max D20",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.WidthMaxD20,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMinDX,
			Name = "Width Min DX",
			Description = "Width Min DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.WidthMinDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthMaxDX,
			Name = "Width Max DX",
			Description = "Width Max DX",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.WidthMaxDX,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.RetentiveAnodeTypeWaitTimer,
			Name = "Retentive Anode Type Wait Timer",
			Description = "Retentive Anode Type Wait Timer",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.RetentiveAnodeTypeWaitTimer,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LengthPresenceAnodeLimit,
			Name = "Length Presence Anode Limit",
			Description = "Length Presence Anode Limit",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LengthPresenceAnodeLimit,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.WidthPresenceAnodeLimit,
			Name = "Width Presence Anode Limit",
			Description = "Width Presence Anode Limit",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.WidthPresenceAnodeLimit,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/

		#endregion

		#region Announcement

		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.EGAMetaDataWait,
			Name = "EGA Meta Data Wait",
			Description = "EGA Meta Data Wait",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.EGAMetaDataWait,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.RetentiveAnodeDetectionTimerZT04,
			Name = "Retentive Anode Detection Timer ZT04",
			Description = "Retentive Anode Detection Timer ZT04",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.RetentiveAnodeDetectionTimerZT04,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/

		#endregion

		#region Health

		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFV01,
			Name = "Delay FV01",
			Description = "Delay FV01",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayFV01,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationFV01,
			Name = "Duration FV01",
			Description = "Duration FV01",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DurationFV01,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFV02,
			Name = "Delay FV02",
			Description = "Delay FV02",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayFV02,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationFV02,
			Name = "Duration FV02",
			Description = "Duration FV02",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DurationFV02,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayFV03,
			Name = "Delay FV03",
			Description = "Delay FV03",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayFV03,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.RetentiveAnodeEntranceTimerZT04,
			Name = "Retentive Anode Entrance Timer ZT04",
			Description = "Retentive Anode Entrance Timer ZT04",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.RetentiveAnodeEntranceTimerZT04,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.CameraCoolingFrequencyNormal,
			Name = "Camera Cooling Frequency Normal",
			Description = "Camera Cooling Frequency Normal",
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
			Name = "Camera Cooling Frequency Hot",
			Description = "Camera Cooling Frequency Hot",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.CameraCoolingFrequencyHot,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LEDBarsCleaningFrequencyNormal,
			Name = "LED Bars Cleaning Frequency Normal",
			Description = "LED Bars Cleaning Frequency Normal",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LEDBarsCleaningFrequencyNormal,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LEDBarsCleaningFrequencyHot,
			Name = "LED Bars Cleaning Frequency Hot",
			Description = "LED Bars Cleaning Frequency Hot",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LEDBarsCleaningFrequencyHot,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.HotAnodeTT02,
			Name = "Hot Anode TT02",
			Description = "Hot Anode TT02",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.HotAnodeTT02,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion

		#region Diagnostic

		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DelayLuxCheck,
			Name = "Delay Lux Check",
			Description = "Delay Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DelayLuxCheck,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.DurationLuxCheck,
			Name = "Duration Lux Check",
			Description = "Duration Lux Check",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.DurationLuxCheck,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ThresholdLuminosityLED,
			Name = "Threshold Luminosity LED",
			Description = "Threshold Luminosity LED",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.ThresholdLuminosityLED,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.ThresholdLuminosityNoLED,
			Name = "Threshold Luminosity No LED",
			Description = "Threshold Luminosity No LED",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.ThresholdLuminosityNoLED,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		/*
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LuminosityWaitTimer,
			Name = "Luminosity Wait Timer",
			Description = "Luminosity Wait Timer",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LuminosityWaitTimer,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});
		*/
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat {
			RID = IOTTagRID.LuminosityCheckFrequency,
			Name = "Luminosity Check Frequency",
			Description = "Luminosity Check Frequency",
			CurrentValue = "0",
			NewValue = "0",
			ValueType = IOTTagType.UShort,
			HasNewValue = false,
			Path = IOTTagPath.LuminosityCheckFrequency,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat,
		});

		#endregion

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

		#endregion

		anodeCTX.SaveChanges();
	}
}
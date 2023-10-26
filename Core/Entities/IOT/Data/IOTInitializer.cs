using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Data;
using Core.Shared.Dictionaries;

namespace Core.Entities.IOT.Data;

public class IOTInitializer
{
	public static void Initialize(AnodeCTX anodeCTX)
	{
		if (anodeCTX.IOTDevice.Any())
			return;
		// Cameras
		InitializeCamera(anodeCTX, DeviceRID.Camera1, "First", 1);
		// InitializeCamera(anodeCTX, DeviceRID.Camera2, "Second", 2);

		// APIs
		InitializeApi(anodeCTX, ITApis.IOTRID, ITApis.IOTAddress, ITApis.IOTPath, true);
		string[] rids =
		{
			ITApis.ADSRID, ITApis.AlarmRID, ITApis.CameraRID, ITApis.CameraAssignRID, ITApis.StationCycleRID,
			ITApis.UserRID
		};
		string[] addresses =
		{
			ITApis.ADSAddress, ITApis.AlarmAddress, ITApis.CameraAddress, ITApis.CameraAssignAddress,
			ITApis.StationCycleAddress, ITApis.UserAddress
		};
		string[] paths =
		{
			ITApis.ADSPath, ITApis.AlarmPath, ITApis.CameraPath, ITApis.CameraAssignPath, ITApis.StationCyclePath,
			ITApis.UserPath
		};
		for (int i = 0; i < rids.Length; ++i)
			InitializeApi(anodeCTX, rids[i], addresses[i], paths[i]);

		// TODO Path.
		InitializeTwinCat(anodeCTX, DeviceRID.TwinCat, ADSUtils.AdsPort.ToString(), "");
	}

	private static void InitializeCamera(AnodeCTX anodeCTX, string rid, string prefix, int suffix)
	{
		OTCamera cam = new()
		{
			RID = rid,
			Name = prefix + " Camera",
			Description = prefix + " Camera IOTDevice",
			Address = "https://localhost:7277",
			ConnectionPath = "/apiCamera/device" + suffix,
			IsConnected = false
		};
		anodeCTX.OTCamera.Add(cam);
		anodeCTX.SaveChanges();
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "TriggerMode" + suffix,
			Name = "Trigger mode",
			Description = "Trigger mode for Camera" + suffix,
			CurrentValue = "On",
			NewValue = "On",
			HasNewValue = true,
			Path = "TriggerMode",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "TriggerSource" + suffix,
			Name = "Trigger source",
			Description = "Trigger source for Camera" + suffix,
			CurrentValue = TriggerSources.Line3,
			NewValue = TriggerSources.Line3,
			HasNewValue = true,
			Path = "TriggerSource",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "TriggerActivation" + suffix,
			Name = "Trigger activation",
			Description = "Trigger activation for Camera" + suffix,
			CurrentValue = TriggerActivations.AnyEdge,
			NewValue = TriggerActivations.AnyEdge,
			HasNewValue = true,
			Path = "TriggerActivation",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "ExposureTime" + suffix,
			Name = "Exposure time",
			Description = "Exposure time for Camera" + suffix,
			CurrentValue = "30000.0",
			NewValue = "30000.0",
			HasNewValue = true,
			Path = "ExposureTime",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "PixelFormat" + suffix,
			Name = "Pixel format",
			Description = "Pixel format for Camera" + suffix,
			CurrentValue = PixelFormats.BayerRG8,
			NewValue = PixelFormats.BayerRG8,
			HasNewValue = true,
			Path = "PixelFormat",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "Width" + suffix,
			Name = "Width",
			Description = "Width for Camera" + suffix,
			CurrentValue = "2464",
			NewValue = "2464",
			HasNewValue = true,
			Path = "Width",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "Height" + suffix,
			Name = "Height",
			Description = "Height for Camera" + suffix,
			CurrentValue = "2056",
			NewValue = "2056",
			HasNewValue = true,
			Path = "Height",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "AcquisitionFrameRateEnable" + suffix,
			Name = "Acquisition frame rate enable",
			Description = "Acquisition frame rate enable for Camera" + suffix,
			CurrentValue = "Off",
			NewValue = "Off",
			HasNewValue = true,
			Path = "AcquisitionFrameRateEnable",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "Gain" + suffix,
			Name = "Gain",
			Description = "Gain for Camera" + suffix,
			CurrentValue = "10",
			NewValue = "10",
			HasNewValue = true,
			Path = "Gain",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "BlackLevel" + suffix,
			Name = "Black level",
			Description = "Black level for Camera" + suffix,
			CurrentValue = "50",
			NewValue = "50",
			HasNewValue = true,
			Path = "BlackLevel",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "Gamma" + suffix,
			Name = "Gamma",
			Description = "Gamma for Camera" + suffix,
			CurrentValue = "1",
			NewValue = "1",
			HasNewValue = true,
			Path = "Gamma",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "BalanceRatio" + suffix,
			Name = "Balance ratio",
			Description = "Balance ratio for Camera" + suffix,
			CurrentValue = "2.35498",
			NewValue = "2.35498",
			HasNewValue = true,
			Path = "BalanceRatio",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "ConvolutionMode" + suffix,
			Name = "Convolution mode",
			Description = "Convolution mode for Camera" + suffix,
			CurrentValue = "Off",
			NewValue = "Off",
			HasNewValue = true,
			Path = "ConvolutionMode",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "AdaptiveNoiseSuppressionFactor" + suffix,
			Name = "Adaptive noise suppression factor",
			Description = "Adaptive noise suppression factor for Camera" + suffix,
			CurrentValue = "1",
			NewValue = "1",
			HasNewValue = true,
			Path = "AdaptiveNoiseSuppressionFactor",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "Sharpness" + suffix,
			Name = "Sharpness",
			Description = "Sharpness for Camera" + suffix,
			CurrentValue = "0",
			NewValue = "0",
			HasNewValue = true,
			Path = "Sharpness",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "AcquisitionFrameRate" + suffix,
			Name = "Acquisition frame rate",
			Description = "Acquisition frame rate for Camera" + suffix,
			CurrentValue = "23.9798",
			NewValue = "23.9798",
			HasNewValue = true,
			Path = "AcquisitionFrameRate",
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
		anodeCTX.SaveChanges();
	}

	private static void InitializeApi(AnodeCTX anodeCTX, string rid, string address, string path,
		bool addTestMode = false)
	{
		ITApi api = new()
		{
			RID = rid,
			Name = rid,
			Description = rid,
			Address = address,
			ConnectionPath = path,
			IsConnected = false
		};
		anodeCTX.ITApi.Add(api);
		anodeCTX.SaveChanges();
		if (addTestMode)
		{
			anodeCTX.IOTTag.Add(new IOTTag
			{
				RID = IOTTagNames.TestModeName,
				Name = IOTTagNames.TestModeName,
				Description = "Test mode tag. Should be the sole one.",
				CurrentValue = "false",
				NewValue = "",
				HasNewValue = false,
				Path = "",
				IOTDeviceID = api.ID,
				IOTDevice = api
			});
			anodeCTX.SaveChanges();
		}
	}

	private static void InitializeTwinCat(AnodeCTX anodeCTX, string rid, string address, string path)
	{
		OTTwinCat twinCat = new()
		{
			RID = rid,
			Name = rid,
			Description = rid,
			Address = address,
			ConnectionPath = path,
			IsConnected = false
		};
		anodeCTX.OTTwinCat.Add(twinCat);
		anodeCTX.SaveChanges();
		// TODO Tags. Mostly paths.
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "Shoot1",
			Name = "Cam 01 - Shoot",
			Description = "Shoot instruction for camera 1",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "Shoot2",
			Name = "Cam 02 - Shoot",
			Description = "Shoot instruction for camera 2",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "LFN01",
			Name = "LED LFN01",
			Description = "LED LFN01",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "LFN02",
			Name = "LED LFN02",
			Description = "LED LFN02",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "LFN03",
			Name = "LED LFN03",
			Description = "LED LFN03",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "LFN04",
			Name = "LED LFN04",
			Description = "LED LFN04",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "FV01",
			Name = "Blowing FV01",
			Description = "Blowing FV01",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "FV02",
			Name = "Blowing FV02",
			Description = "Blowing FV02",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.OTTagTwinCat.Add(new OTTagTwinCat
		{
			RID = "FV03",
			Name = "Blowing FV03",
			Description = "Blowing FV03",
			CurrentValue = "false",
			NewValue = "false",
			ValueType = IOTTagType.Bool,
			HasNewValue = false,
			IOTDeviceID = twinCat.ID,
			IOTDevice = twinCat
		});
		anodeCTX.SaveChanges();
	}
}
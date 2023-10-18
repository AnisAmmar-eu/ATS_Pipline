using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Data;

namespace Core.Entities.IOT.Data;

public class IOTInitializer
{
	public static void Initialize(AnodeCTX anodeCTX)
	{
		if (anodeCTX.IOTDevice.Any())
			return;
		InitializeCamera(anodeCTX, "First", 1);
		// InitializeCamera(anodeCTX, "2", "Second");
	}

	private static void InitializeCamera(AnodeCTX anodeCTX, string prefix, int suffix)
	{
		OTCamera cam = new()
		{
			RID = "Camera" + suffix,
			Name = prefix + " Camera",
			Description = prefix + " Camera IOTDevice",
			Address = "https://localhost:7253",
			IsConnected = false
		};
		anodeCTX.IOTDevice.Add(cam);
		anodeCTX.SaveChanges();
		anodeCTX.IOTTag.Add(new IOTTag
		{
			RID = "Connection" + suffix,
			Name = IOTTagNames.CheckConnectionName,
			Description = "Connection tag for Camera" + suffix,
			CurrentValue = "On",
			NewValue = "",
			HasNewValue = false,
			Path = "/device" + suffix,
			IOTDeviceID = cam.ID,
			IOTDevice = cam
		});
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
}
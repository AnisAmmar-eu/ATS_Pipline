namespace Core.Entities.IOT.Dictionaries;

public class IOTTagType
{
	public static string String = "string";
	public static string Int = "int";
	public static string UShort = "ushort";
	public static string Bool = "bool";
}

public static class IOTTagRID
{
	public const string TestMode = "__TestMode";

	#region Camera

	public const string TriggerMode = "TriggerMode";
	public const string TriggerSource = "TriggerSource";
	public const string TriggerActivation = "TriggerActivation";
	public const string ExposureTime = "ExposureTime";
	public const string PixelFormat = "PixelFormat";
	public const string Width = "Width";
	public const string Height = "Height";
	public const string AcquisitionFrameRateEnable = "AcquisitionFrameRateEnable";
	public const string Gain = "Gain";
	public const string BlackLevel = "BlackLevel";
	public const string Gamma = "Gamma";
	public const string BalanceRatio = "BalanceRatio";
	public const string ConvolutionMode = "ConvolutionMode";
	public const string AdaptiveNoiseSuppressionFactor = "AdaptiveNoiseSuppressionFactor";
	public const string Sharpness = "Sharpness";
	public const string AcquisitionFrameRate = "AcquisitionFrameRate";

	#endregion

	#region TestMode

	public const string Shoot1 = "Shoot1";
	public const string Shoot2 = "Shoot2";
	public const string Led1 = "LFN01";
	public const string Led2 = "LFN02";
	public const string Led3 = "LFN03";
	public const string Led4 = "LFN04";
	public const string Blowing1 = "FV01";
	public const string Blowing2 = "FV02";
	public const string Blowing3 = "FV03";

	public const string SequencePicture1 = "SequencePicture1";
	public const string SequencePicture2 = "SequencePicture2";
	public const string SequenceCleaning = "SequenceCleaning";
	public const string SequenceCooling = "SequenceCooling";
	public const string SequencePressure = "SequencePressure";
	public const string SequenceLEDOff = "SequenceLEDOff";
	public const string SequenceLEDOn = "SequenceLEDOn";

	#endregion

	#region Shooting

	public const string RetentiveShootingWaitTimer = "RetentiveShootingWaitTimer";

	public const string DelayFlashD20 = "DelayFlashD20";
	public const string DelayFlashDX = "DelayFlashDX";
	public const string DelayFlashInvalid = "DelayFlashInvalid";

	public const string DurationFlashD20 = "DurationFlashD20";

	public const string DelayCamD20 = "DelayCamD20";
	public const string DelayCamDX = "DelayCamDX";
	public const string DelayCamInvalid = "DelayCamInvalid";

	public const string TriggerThreshold1DX = "TriggerThreshold1DX";
	public const string TriggerThreshold1D20 = "TriggerThreshold1D20";
	public const string TriggerThreshold2DX = "TriggerThreshold2DX";
	public const string TriggerThreshold2D20 = "TriggerThreshold2D20";
	public const string TriggerThreshold3DX = "TriggerThreshold3DX";
	public const string TriggerThreshold3D20 = "TriggerThreshold3D20";

	public const string DelayValidLaser = "DelayValidLaser";
	public const string TransferTimer = "TransferTimer";

	#endregion

	#region Anode

	public const string LengthMinD20 = "LengthMinD20";
	public const string LengthMaxD20 = "LengthMaxD20";
	public const string LengthMinDX = "LengthMinDX";
	public const string LengthMaxDX = "LengthMaxDX";

	public const string WidthMinD20 = "WidthMinD20";
	public const string WidthMaxD20 = "WidthMaxD20";
	public const string WidthMinDX = "WidthMinDX";
	public const string WidthMaxDX = "WidthMaxDX";

	public const string RetentiveAnodeTypeWaitTimer = "RetentiveAnodeTypeWaitTimer";
	public const string LengthPresenceAnodeLimit = "LengthPresenceAnodeLimit";
	public const string WidthPresenceAnodeLimit = "WidthPresenceAnodeLimit";

	#endregion

	#region Announcement

	public const string EGAMetaDataWait = "EGAMetaDataWait";
	public const string RetentiveAnodeDetectionTimerZT04 = "RetentiveAnodeDetectionTimerZT04";

	#endregion

	#region Health

	public const string DelayFV01 = "DelayFV01";
	public const string DurationFV01 = "DurationFV01";
	public const string DelayFV02 = "DelayFV02";
	public const string DurationFV02 = "DurationFV02";
	public const string DelayFV03 = "DelayFV03";

	public const string RetentiveAnodeEntranceTimerZT04 = "RetentiveAnodeEntranceTimerZT04";
	public const string CameraCoolingFrequencyNormal = "CameraCoolingFrequencyNormal";
	public const string CameraCoolingFrequencyHot = "CameraCoolingFrequencyHot";
	public const string LEDBarsCleaningFrequencyNormal = "LEDBarsCleaningFrequencyNormal";
	public const string LEDBarsCleaningFrequencyHot = "LEDBarsCleaningFrequencyHot";
	public const string HotAnodeTT02 = "HotAnodeTT02";

	#endregion

	#region Diagnostic

	public const string DelayLuxCheck = "DelayLuxCheck";
	public const string DurationLuxCheck = "DurationLuxCheck";
	public const string ThresholdLuminosityLED = "ThresholdLuminosityLED";
	public const string ThresholdLuminosityNoLED = "ThresholdLuminosityNoLED";
	public const string LuminosityWaitTimer = "LuminosityWaitTimer";
	public const string LuminosityCheckFrequency = "LuminosityCheckFrequency";

	#endregion
}

public static class IOTTagPath
{
	#region Camera

	public const string TriggerMode = "TriggerMode";
	public const string TriggerSource = "TriggerSource";
	public const string TriggerActivation = "TriggerActivation";
	public const string ExposureTime = "ExposureTime";
	public const string PixelFormat = "PixelFormat";
	public const string Width = "Width";
	public const string Height = "Height";
	public const string AcquisitionFrameRateEnable = "AcquisitionFrameRateEnable";
	public const string Gain = "Gain";
	public const string BlackLevel = "BlackLevel";
	public const string Gamma = "Gamma";
	public const string BalanceRatio = "BalanceRatio";
	public const string ConvolutionMode = "ConvolutionMode";
	public const string AdaptiveNoiseSuppressionFactor = "AdaptiveNoiseSuppressionFactor";
	public const string Sharpness = "Sharpness";
	public const string AcquisitionFrameRate = "AcquisitionFrameRate";

	#endregion

	#region TestMode

	public const string Shoot1 = "";
	public const string Shoot2 = "";
	public const string Led1 = "";
	public const string Led2 = "";
	public const string Led3 = "";
	public const string Led4 = "";
	public const string Blowing1 = "";
	public const string Blowing2 = "";
	public const string Blowing3 = "";

	public const string SequencePicture1 = "";
	public const string SequencePicture2 = "";
	public const string SequenceCleaning = "";
	public const string SequenceCooling = "";
	public const string SequencePressure = "";
	public const string SequenceLEDOff = "";
	public const string SequenceLEDOn = "";

	#endregion

	#region Shooting

	public const string RetentiveShootingWaitTimer = "";

	public const string DelayFlashD20 = "";
	public const string DelayFlashDX = "";
	public const string DelayFlashInvalid = "";

	public const string DurationFlashD20 = "";

	public const string DelayCamD20 = "";
	public const string DelayCamDX = "";
	public const string DelayCamInvalid = "";

	public const string TriggerThreshold1DX = "";
	public const string TriggerThreshold1D20 = "";
	public const string TriggerThreshold2DX = "";
	public const string TriggerThreshold2D20 = "";
	public const string TriggerThreshold3DX = "";
	public const string TriggerThreshold3D20 = "";

	public const string DelayValidLaser = "";
	public const string TransferTimer = "";

	#endregion

	#region Anode

	public const string LengthMinD20 = "";
	public const string LengthMaxD20 = "";
	public const string LengthMinDX = "";
	public const string LengthMaxDX = "";

	public const string WidthMinD20 = "";
	public const string WidthMaxD20 = "";
	public const string WidthMinDX = "";
	public const string WidthMaxDX = "";

	public const string RetentiveAnodeTypeWaitTimer = "";
	public const string LengthPresenceAnodeLimit = "";
	public const string WidthPresenceAnodeLimit = "";

	#endregion

	#region Announcement

	public const string EGAMetaDataWait = "";
	public const string RetentiveAnodeDetectionTimerZT04 = "";

	#endregion

	#region Health

	public const string DelayFV01 = "";
	public const string DurationFV01 = "";
	public const string DelayFV02 = "";
	public const string DurationFV02 = "";
	public const string DelayFV03 = "";

	public const string RetentiveAnodeEntranceTimerZT04 = "";
	public const string CameraCoolingFrequencyNormal = "";
	public const string CameraCoolingFrequencyHot = "";
	public const string LEDBarsCleaningFrequencyNormal = "";
	public const string LEDBarsCleaningFrequencyHot = "";
	public const string HotAnodeTT02 = "";

	#endregion

	#region Diagnostic

	public const string DelayLuxCheck = "";
	public const string DurationLuxCheck = "";
	public const string ThresholdLuminosityLED = "";
	public const string ThresholdLuminosityNoLED = "";
	public const string LuminosityWaitTimer = "";
	public const string LuminosityCheckFrequency = "";

	#endregion
}
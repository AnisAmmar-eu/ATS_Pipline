namespace Core.Entities.IOT.Dictionaries;

public static class IOTTagType
{
	public const string String = "string";
	public const string Int = "int";
	public const string UInt = "uint";
	public const string Float = "float";
	public const string Double = "double";
	public const string UShort = "ushort";
	public const string Bool = "bool";
}

public static class IOTTagRID
{
	public const string TestMode = "__TestMode";

	#region Camera

	public const string TriggerSource = "TriggerSource";
	public const string TriggerActivation = "TriggerActivation";
	public const string ExposureTime = "ExposureTime";
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

	// Those are handled in the TwinCat

	#region Camera Temperature

	public const string TemperatureCam1 = "TemperatureCam1";
	public const string TemperatureCam2 = "TemperatureCam2";
	public const string TemperatureOkWarnThreshold = "TemperatureOkWarnThreshold";
	public const string TemperatureWarnErrorThreshold = "TemperatureWarnErrorThreshold";
	public const string TemperatureStatusCam1 = "TemperatureStatusCam1";
	public const string TemperatureStatusCam2 = "TemperatureStatusCam2";

	#endregion

	#region Status

	public const string TSH01 = "TSH01";
	public const string PowerFailure = "PowerFailure";
	public const string AirPressure = "AirPressure";

	public const string TT01 = "TT01";
	public const string TT02 = "TT02";

	public const string DiagCam1LedOn = "DiagCam1LedOn";
	public const string DiagCam1LedOff = "DiagCam1LedOff";
	public const string DiagCam2LedOn = "DiagCam2LedOn";
	public const string DiagCam2LedOff = "DiagCam2LedOff";

	public const string Cam1Status = "Cam1Status";
	public const string Cam2Status = "Cam2Status";

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
	public const string SequenceCleaningCam1 = "SequenceCleaningCam1";
	public const string SequenceCleaningCam2 = "SequenceCleaningCam2";
	public const string SequenceCooling = "SequenceCooling";
	public const string SequenceCam1LedOn = "SequenceCam1LedOn";
	public const string SequenceCam1LedOff = "SequenceCam1LedOff";
	public const string SequenceCam2LedOn = "SequenceCam2LedOn";
	public const string SequenceCam2LedOff = "SequenceCam2LedOff";

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

	#region Lasers

	public const string ZT1 = "ZT1";
	public const string ZT2 = "ZT2";
	public const string ZT3 = "ZT3";
	public const string ZT4 = "ZT4";

	#endregion
}

public static class IOTTagPath
{
	#region Camera

	public const string TriggerSource = "TriggerSource";
	public const string TriggerActivation = "TriggerActivation";
	public const string ExposureTime = "ExposureTime";
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

	#region Camera Temperature

	public const string TemperatureCam1 = "VA_DM.CAM001_Temp";
	public const string TemperatureCam2 = "VA_DM.CAM002_Temp";
	public const string TemperatureOkWarnThreshold = "";
	public const string TemperatureWarnErrorThreshold = "";
	public const string TemperatureStatusCam1 = "VA_HMI.Cam01_Temp_Status";
	public const string TemperatureStatusCam2 = "VA_HMI.Cam02_Temp_Status";

	#endregion

	#region Status

	public const string TSH01 = "VA_HMI.TSH01";
	public const string PowerFailure = "VA_HMI.PW_F";
	public const string AirPressure = "VA_HMI.PSL01";

	public const string TT01 = "VA_HMI.TT01";
	public const string TT02 = "VA_HMI.TT02";

	public const string DiagCam1LedOn = "VA_HMI.Cam01_Led_ON";
	public const string DiagCam1LedOff = "VA_HMI.Cam01_Led_OFF";
	public const string DiagCam2LedOn = "VA_HMI.Cam02_Led_ON";
	public const string DiagCam2LedOff = "VA_HMI.Cam02_Led_OFF";

	public const string Cam1Status = "VA_DM.CAM001_Status";
	public const string Cam2Status = "VA_DM.CAM002_Status";

	#endregion

	#region TestMode

	public const string TestMode = "VA_HMI.TestMode";

	public const string Shoot1 = "VA_HMI.Cam01";
	public const string Shoot2 = "VA_HMI.Cam02";
	public const string Led1 = "VA_HMI.LFN01";
	public const string Led2 = "VA_HMI.LFN02";
	public const string Led3 = "VA_HMI.LFN03";
	public const string Led4 = "VA_HMI.LFN04";
	public const string Blowing1 = "VA_HMI.FV01";
	public const string Blowing2 = "VA_HMI.FV02";
	public const string Blowing3 = "VA_HMI.FV03";

	public const string SequencePicture1 = "VA_HMI.Shooting_Cam01";
	public const string SequencePicture2 = "VA_HMI.Shooting_Cam02";
	public const string SequenceCleaningCam1 = "VA_HMI.Clean_Cam01";
	public const string SequenceCleaningCam2 = "VA_HMI.Clean_Cam02";
	public const string SequenceCooling = "VA_HMI.Cool_Cam01";
	public const string SequenceCam1LedOn = "VA_HMI.DIAG_CAM01_LED_ON";
	public const string SequenceCam1LedOff = "VA_HMI.DIAG_CAM01_LED_OFF";
	public const string SequenceCam2LedOn = "VA_HMI.DIAG_CAM02_LED_ON";
	public const string SequenceCam2LedOff = "VA_HMI.DIAG_CAM02_LED_OFF";

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

	#region Lasers

	public const string ZT1 = "VA_HMI.ZT01";
	public const string ZT2 = "VA_HMI.ZT02";
	public const string ZT3 = "VA_HMI.ZT03";
	public const string ZT4 = "VA_HMI.ZT04";

	#endregion
}
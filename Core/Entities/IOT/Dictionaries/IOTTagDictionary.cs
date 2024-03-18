namespace Core.Entities.IOT.Dictionaries;

public static class IOTTagType
{
	public const string String = "string"; // STRING TwinCat
	public const string Int = "int"; // DINT TwinCat
	public const string Float = "float"; // REAL TwinCat
	public const string UShort = "ushort"; // UINT TwinCat
	public const string Bool = "bool"; // BOOL TwinCat
}

public static class IOTTagRID
{
	public const string TestMode = "__TestMode";
	public const string MsgInit = "MsgInit";

	#region Camera

	public const string TriggerMode = "TriggerMode";
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
	public const string SequenceCleaningCam = "SequenceCleaningCam";
	public const string SequenceCoolingCam = "SequenceCoolingCam";
	public const string SequenceCleanLed = "SequenceCleanLed";
	public const string SequenceCam1LedOn = "SequenceCam1LedOn";
	public const string SequenceCam1LedOff = "SequenceCam1LedOff";
	public const string SequenceCam2LedOn = "SequenceCam2LedOn";
	public const string SequenceCam2LedOff = "SequenceCam2LedOff";

	#endregion

	#region Shooting

	public const string DelayFlashD20 = "DelayFlashD20";
	public const string DurationFlashD20 = "DurationFlashD20";
	public const string HightTopD20 = "HightTopD20";
	public const string HightMinD20 = "HightMinD20";
	public const string HoleMaxD20 = "HoleMaxD20";
	public const string HoleMinD20 = "HoleMinD20";
	public const string DelayCamD20 = "DelayCamD20";

	public const string DelayFlashDX = "DelayFlashDX";
	public const string DurationFlashDX = "DurationFlashDX";
	public const string DelayCamDX = "DelayCamDX";
	public const string HightTopDX = "HightTopDX";
	public const string HightMinDX = "HightMinDX";
	public const string HoleMaxDX = "HoleMaxDX";
	public const string HoleMinDX = "HoleMinDX";

	#endregion

	#region Anode

	public const string LengthMinD20 = "LengthMinD20";
	public const string LengthMaxD20 = "LengthMaxD20";
	public const string WidthMinD20 = "WidthMinD20";
	public const string WidthMaxD20 = "WidthMaxD20";
	public const string InHoleDelayD20 = "InHoleDelayD20";
	public const string WidthDelayMaxD20 = "WidthDelayMaxD20";
	public const string LengthDelayMaxD20 = "LengthDelayMaxD20";

	// Assuming similar naming convention for DX as D20
	public const string LengthMinDX = "LengthMinDX";
	public const string LengthMaxDX = "LengthMaxDX";
	public const string WidthMinDX = "WidthMinDX";
	public const string WidthMaxDX = "WidthMaxDX";
	public const string InHoleDelayDX = "InHoleDelayDX";
	public const string WidthDelayMaxDX = "WidthDelayMaxDX";
	public const string LengthDelayMaxDX = "LengthDelayMaxDX";

	#endregion

	#region Announcement

	public const string EGAMetaDataWait = "EGAMetaDataWait";
	public const string RetentiveAnodeDetectionTimerZT04 = "RetentiveAnodeDetectionTimerZT04";

	#endregion

	#region Health

	public const string CameraCoolingPeriod = "CameraCoolingPeriod";
	public const string CameraCoolingFrequencyNormal = "CameraCoolingFrequencyNormal";
	public const string CameraCoolingFrequencyHot = "CameraCoolingFrequencyHot";
	public const string CameraCoolingTempWarning = "CameraCoolingTempWarning";
	public const string CameraCoolingDuration = "CameraCoolingDuration";

	public const string LedBlowingPeriod = "LedBlowingPeriod";
	public const string LedBlowingFrequencyNormal = "LedBlowingFrequencyNormal";
	public const string LedBlowingTempWarning = "LedBlowingTempWarning";
	public const string LedBlowingFrequencyHot = "LedBlowingFrequencyHot";
	public const string LedBlowingDuration = "LedBlowingDuration";

	public const string CleanTopDetectDelay = "CleanTopDetectDelay";
	public const string CleanTopDelay = "CleanTopDelay";
	public const string CleanTopDuration = "CleanTopDuration";

	public const string HotAnodeTT02 = "HotAnodeTT02";
	public const string WarnCam01Temp = "WarnCam01Temp";
	public const string WarnCam02Temp = "WarnCam02Temp";

	#endregion

	#region Diagnostic

	public const string LuminosityCheckFrequencyCam1LedOn = "LuminosityCheckFrequencyCam1LedOn";
	public const string ThresholdLuminosityCam1LedOn = "ThresholdLuminosityCam1LedOn";
	public const string DurationLuxCheckCam1LedOn = "DurationLuxCheckCam1LedOn";

	public const string ThresholdLuminosityCam1LedOff = "ThresholdLuminosityCam1LedOff";
	public const string LuminosityCheckFrequencyCam1LedOff = "LuminosityCheckFrequencyCam1LedOff";
	public const string DurationLuxCheckCam1LedOff = "DurationLuxCheckCam1LedOff";

	public const string LuminosityCheckFrequencyCam2LedOn = "LuminosityCheckFrequencyCam2LedOn";
	public const string ThresholdLuminosityCam2LedOn = "ThresholdLuminosityCam2LedOn";
	public const string DurationLuxCheckCam2LedOn = "DurationLuxCheckCam2LedOn";

	public const string ThresholdLuminosityCam2LedOff = "ThresholdLuminosityCam2LedOff";
	public const string LuminosityCheckFrequencyCam2LedOff = "LuminosityCheckFrequencyCam2LedOff";
	public const string DurationLuxCheckCam2LedOff = "DurationLuxCheckCam2LedOff";

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

	public const string TriggerMode = "TriggerMode";
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

	public const string TemperatureCam1Write = "VA_IT.CAM01_Temp";
	public const string TemperatureCam2Write = "VA_IT.CAM02_Temp";
	public const string TemperatureCam1Read = "VA_HMI.Cam01_Temp";
	public const string TemperatureCam2Read = "VA_HMI.Cam02_Temp";
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

	public const string Cam1StatusWrite = "VA_IT.CAM01_StatusOK";
	public const string Cam2StatusWrite = "VA_IT.CAM02_StatusOK";
	public const string Cam1StatusRead = "VA_HMI.Cam01_Status";
	public const string Cam2StatusRead = "VA_HMI.Cam02_Status";

	#endregion

	#region TestMode

	public const string TestMode = "VA_HMI.TestMode";
	public const string MsgInit = "VA_HMI.MsgInit";

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
	public const string SequenceCleaningCam = "VA_HMI.Clean_Cam";
	public const string SequenceCoolingCam = "VA_HMI.Cool_Cam";
	public const string SequenceCleanLed = "VA_HMI.Clean_Led";
	public const string SequenceCam1LedOn = "VA_HMI.DIAG_CAM01_LED_ON";
	public const string SequenceCam1LedOff = "VA_HMI.DIAG_CAM01_LED_OFF";
	public const string SequenceCam2LedOn = "VA_HMI.DIAG_CAM02_LED_ON";
	public const string SequenceCam2LedOff = "VA_HMI.DIAG_CAM02_LED_OFF";

	#endregion

	#region Shooting

	public const string DelayFlashD20 = "VA_SETTINGS.Anode_D20.Delay_Flash";
	public const string DurationFlashD20 = "VA_SETTINGS.Anode_D20.Duration_Flash";
	public const string DelayCamD20 = "VA_SETTINGS.Anode_D20.Delay_Cam";
	public const string HightTopD20 = "VA_SETTINGS.Anode_D20.Hight_Top";
	public const string HightMinD20 = "VA_SETTINGS.Anode_D20.Hight_Min";
	public const string HoleMaxD20 = "VA_SETTINGS.Anode_D20.Hole_Max";
	public const string HoleMinD20 = "VA_SETTINGS.Anode_D20.Hole_Min";

	public const string DelayFlashDX = "VA_SETTINGS.Anode_DX.Delay_Flash";
	public const string DurationFlashDX = "VA_SETTINGS.Anode_DX.Duration_Flash";
	public const string DelayCamDX = "VA_SETTINGS.Anode_DX.Delay_Cam";
	public const string HightTopDX = "VA_SETTINGS.Anode_DX.Hight_Top";
	public const string HightMinDX = "VA_SETTINGS.Anode_DX.Hight_Min";
	public const string HoleMaxDX = "VA_SETTINGS.Anode_DX.Hole_Max";
	public const string HoleMinDX = "VA_SETTINGS.Anode_DX.Hole_Min";

	#endregion

	#region Anode

	public const string LengthMaxD20 = "VA_SETTINGS.Anode_D20.Lenght_Max";
	public const string LengthMinD20 = "VA_SETTINGS.Anode_D20.Lenght_Min";
	public const string WidthMaxD20 = "VA_SETTINGS.Anode_D20.Width_Max";
	public const string WidthMinD20 = "VA_SETTINGS.Anode_D20.Width_Min";

	public const string InHoleDelayD20 = "VA_SETTINGS.Anode_D20.In_Hole_Delay";
	public const string WidthDelayMaxD20 = "VA_SETTINGS.Anode_D20.Width_Delay_Max";
	public const string LengthDelayMaxD20 = "VA_SETTINGS.Anode_D20.Lenght_Delay_Max";

	public const string LengthMaxDX = "VA_SETTINGS.Anode_DX.Lenght_Max";
	public const string LengthMinDX = "VA_SETTINGS.Anode_DX.Lenght_Min";
	public const string WidthMaxDX = "VA_SETTINGS.Anode_DX.Width_Max";
	public const string WidthMinDX = "VA_SETTINGS.Anode_DX.Width_Min";

	public const string InHoleDelayDX = "VA_SETTINGS.Anode_DX.In_Hole_Delay";
	public const string WidthDelayMaxDX = "VA_SETTINGS.Anode_DX.Width_Delay_Max";
	public const string LengthDelayMaxDX = "VA_SETTINGS.Anode_DX.Lenght_Delay_Max";

	#endregion

	#region Health

	public const string CameraCoolingPeriod = "VA_SETTINGS.Health_Cam_Cooling_FV01.Period";
	public const string CameraCoolingFrequencyNormal = "VA_SETTINGS.Health_Cam_Cooling_FV01.SP";
	public const string CameraCoolingFrequencyHot = "VA_SETTINGS.Health_Cam_Cooling_FV01.SP_Warning";
	public const string CameraCoolingTempWarning = "VA_SETTINGS.Health_Cam_Cooling_FV01.Temp_Warning";
	public const string CameraCoolingDuration = "VA_SETTINGS.Health_Cam_Cooling_FV01.Duration";

	public const string LedBlowingPeriod = "VA_SETTINGS.Health_Cam_Led_Blowing_FV02.Period";
	public const string LedBlowingFrequencyNormal = "VA_SETTINGS.Health_Cam_Led_Blowing_FV02.SP";
	public const string LedBlowingFrequencyHot = "VA_SETTINGS.Health_Cam_Led_Blowing_FV02.SP_Warning";
	public const string LedBlowingTempWarning = "VA_SETTINGS.Health_Cam_Led_Blowing_FV02.Temp_Warning";
	public const string LedBlowingDuration = "VA_SETTINGS.Health_Cam_Led_Blowing_FV02.Duration";

	public const string CleanTopDetectDelay = "VA_SETTINGS.Clean_Top_Dectection_Delay";
	public const string CleanTopDelay = "VA_SETTINGS.Clean_Waitting_Delay";
	public const string CleanTopDuration = "VA_SETTINGS.Clean_Duration";
	public const string HotAnodeTT02 = "VA_SETTINGS.Health_S5_Cool_AnodeTemp_SP";
	public const string WarnCam01Temp = "VA_SETTINGS.CAM01_Temp_H";
	public const string WarnCam02Temp = "VA_SETTINGS.CAM02_Temp_H";

	#endregion

	#region Diagnostic

	// Cam1LedOn
	public const string LuminosityCheckFrequencyCam1LedOn = "VA_SETTINGS.CAM01_LED_ON.Counter_SP";
	public const string ThresholdLuminosityCam1LedOn = "VA_SETTINGS.CAM01_LED_ON.SP";
	public const string DurationLuxCheckCam1LedOn = "VA_SETTINGS.CAM01_LED_ON.Duration";

	// Cam1 LEDOFF
	public const string ThresholdLuminosityCam1LedOff = "VA_SETTINGS.CAM01_LED_OFF.SP";
	public const string LuminosityCheckFrequencyCam1LedOff = "VA_SETTINGS.CAM01_LED_OFF.Counter_SP";
	public const string DurationLuxCheckCam1LedOff = "VA_SETTINGS.CAM01_LED_OFF.Duration";

	// Cam2 LEDON
	public const string ThresholdLuminosityCam2LedOn = "VA_SETTINGS.CAM02_LED_ON.SP";
	public const string LuminosityCheckFrequencyCam2LedOn = "VA_SETTINGS.CAM02_LED_ON.Counter_SP";
	public const string DurationLuxCheckCam2LedOn = "VA_SETTINGS.CAM02_LED_ON.Duration";

	// Cam2 LEDOFF
	public const string ThresholdLuminosityCam2LedOff = "VA_SETTINGS.CAM02_LED_OFF.SP";
	public const string LuminosityCheckFrequencyCam2LedOff = "VA_SETTINGS.CAM02_LED_OFF.Counter_SP";
	public const string DurationLuxCheckCam2LedOff = "VA_SETTINGS.CAM02_LED_OFF.Duration";

	#endregion

	#region Lasers

	public const string ZT1 = "VA_HMI.ZT01";
	public const string ZT2 = "VA_HMI.ZT02";
	public const string ZT3 = "VA_HMI.ZT03";
	public const string ZT4 = "VA_HMI.ZT04";

	#endregion
}
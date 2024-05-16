namespace Core.Shared.Dictionaries;

/// <summary>
/// Provides a set of constants for configurations.
/// </summary>
public static class ConfigDictionary
{
	#region Program

	public const string ClientHost = "ClientHost";

	#endregion Program

	#region Api Addresses

	public const string ApiADSAddress = "ApiAddresses:ApiADS";
	public const string ApiAlarmAddress = "ApiAddresses:ApiAlarm";
	public const string ApiCameraAddress = "ApiAddresses:ApiCamera";
	public const string ApiIOTAddress = "ApiAddresses:ApiIOT";
	public const string ApiKPIAddress = "ApiAddresses:ApiKPI";
	public const string ApiServerReceiveAddress = "ApiAddresses:ApiServerReceive";
	public const string ApiStationCycleAddress = "ApiAddresses:ApiStationCycle";
	public const string ApiUserAddress = "ApiAddresses:ApiUser";
	public const string ApiVisionAddress = "ApiAddresses:ApiVision";

	#endregion Api Addresses

	#region Camera Config

	public const string Camera1Port = "CameraConfig:Camera1:Port";
	public const string Camera2Port = "CameraConfig:Camera2:Port";
	public const string TimeOutCamera = "CameraConfig:TimeOut";

	public const string ThumbnailsPath = "CameraConfig:ThumbnailsPath";
	public const string ImagesPath = "CameraConfig:ImagesPath";
	public const string ImagesOutputPath = "CameraConfig:ImagesOutputPath";
	public const string CameraExtension = "CameraConfig:Extension";

	#endregion Camera Config

	#region Station Config

	public const string StationName = "StationConfig:StationName";

	#endregion Station Config

	#region Vision settings

	//Common
	public const string FolderParams = "VisionSettings:FolderParams";
	public const string StaticSignName = "signature_static.xml";

	public const string DLLPath = "VisionSettings:DLLPath";
	public const string SignMatchTimer = "VisionSettings:SignMatchTimer";

	//sign
	public const string DynamicSignName = "signature_dynamic.xml";
	public const string CameraID = "VisionSettings:CameraID";
	public const string AnodeType = "VisionSettings:AnodeType";

	public const string LoadDestinations = "VisionSettings:LoadDestinations";
	public const string MatchDestinations = "VisionSettings:MatchDestinations";
	// OriginGateIDs are directly in configuration, no need to be loaded

	//match
	public const string DynamicMatchName = "match_dynamic.xml";
	public const string InstanceMatchID = "VisionSettings:InstanceMatchID";
	public const string StationDelay = "VisionSettings:StationDelay";
	public const string GPUID = "VisionSettings:GPUID";
	public const string GoMatchStations = "VisionSettings:GoMatchStations";
	public const string IsChained = "VisionSettings:IsChained";

	#endregion Vision settings

	#region Vision File settings

	public const string FileSettingTimer = "FileSettings:FileSettingTimer";
	public const string ArchivePath = "FileSettings:ArchivePath";

	#endregion Vision File settings

	#region CheckSyncTime

	public const string DeltaTimeSec = "DeltaTimeSec";

	#endregion CheckSyncTime

	#region DiskCheck

	public const string DiskCheckThreshold = "DiskCheckThreshold";
	public const string DiskCheckLabel = "DiskCheckLabel";

	#endregion DiskCheck

	#region Purge

	public const string PurgeTimerSec = "PurgeTimerSec";
	public const string PurgeThreshold = "PurgeThreshold";
	public const string PurgeRawPictures = "PurgeRawPictures";
	public const string PurgeMetadata = "PurgeMetadata";
	public const string PurgeAnodeEntry = "PurgeAnodeEntry";
	public const string PurgeCycle = "PurgeCycle";

	#endregion Purge

	#region MS settings

	public const string CycleMS = "CycleMS";
	public const string RetryMS = "RetryMS";
	public const string CheckSyncTimeMS = "CheckSyncTimeMS";
	public const string IOTMS = "IOTMS";
	public const string SendAlarmCycleMS = "SendAlarmCycleMS";
	public const string SendPacketMS = "SendPacketMS";
	public const string SendLogMS = "SendLogMS";
	public const string WatchdogDelay = "WatchdogDelay";

	#endregion MS settings
}
namespace Core.Shared.Dictionaries;

/// <summary>
/// Provides a set of constants for configurations.
/// </summary>
public static class ConfigDictionary
{
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

	#endregion

	#region Camera Config

	public const string Camera1Port = "CameraConfig:Camera1:Port";
	public const string Camera2Port = "CameraConfig:Camera2:Port";

	public const string ThumbnailsPath = "CameraConfig:ThumbnailsPath";
	public const string ImagesPath = "CameraConfig:ImagesPath";
	public const string CameraExtension = "CameraConfig:Extension";

	#endregion

	#region Station Config

	public const string StationName = "StationConfig:StationName";

	#endregion

	#region Vision settings

	public const string S3S4Delay = "VisionSettings:S3S4Delay";
	public const string S5Delay = "VisionSettings:S5Delay";

	#endregion
}
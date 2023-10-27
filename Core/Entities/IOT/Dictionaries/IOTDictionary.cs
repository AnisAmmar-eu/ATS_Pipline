namespace Core.Entities.IOT.Dictionaries;

public static class DeviceRID
{
	public static string Camera1 = "Camera1";
	public static string Camera2 = "Camera2";

	public static string TwinCat = "TwinCat";
}

public static class ITApis
{
	public static string ADSRID = "ApiADS";
	public static string ADSAddress = "https://localhost:7275"; // 5242
	public static string ADSPath = "/apiADS/status";

	public static string AlarmRID = "ApiAlarm";
	public static string AlarmAddress = "https://localhost:7276"; // 5243
	public static string AlarmPath = "/apiAlarm/status";

	public static string CameraRID = "ApiCamera";
	public static string CameraAddress = "https://localhost:7277"; // 5244
	public static string CameraPath = "/apiCamera/status";

	public static string CameraAssignRID = "ApiCameraAssign";
	public static string CameraAssignAddress = "https://localhost:7278"; // 5245
	public static string CameraAssignPath = "/apiCameraAssign/status";

	public static string IOTRID = "ApiIOT";
	public static string IOTAddress = "https://localhost:7279"; // 5246
	public static string IOTPath = "/apiIOT/status";

	// 7280 & 5247 is taken by ApiServerReceive

	public static string StationCycleRID = "ApiStationCycle";
	public static string StationCycleAddress = "https://localhost:7281"; // 5248
	public static string StationCyclePath = "/apiStationCycle/status";

	public static string UserRID = "ApiUser";
	public static string UserAddress = "https://localhost:7282"; // 5249
	public static string UserPath = "/apiUser/status";
}

public static class TriggerSources
{
	public const string Line3 = "Line3";
}

public static class TriggerActivations
{
	public const string AnyEdge = "AnyEdge";
}

public static class PixelFormats
{
	public const string BayerRG8 = "BayerRG8";
}
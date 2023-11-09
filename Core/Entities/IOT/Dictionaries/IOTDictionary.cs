namespace Core.Entities.IOT.Dictionaries;

public static class DeviceRID
{
	public const string Camera1 = "Camera1";
	public const string Camera2 = "Camera2";

	public const string TwinCat = "TwinCat";
}

public static class ITApisDict
{
	public const string ADSRID = "ApiADS";
	public const string ADSAddress = "https://localhost:7275"; // 5242
	public const string ADSPath = "/apiADS/status";

	public const string AlarmRID = "ApiAlarm";
	public const string AlarmAddress = "https://localhost:7276"; // 5243
	public const string AlarmPath = "/apiAlarm/status";

	public const string CameraRID = "ApiCamera";
	public const string CameraAddress = "https://localhost:7277"; // 5244
	public const string CameraPath = "/apiCamera/status";

	public const string CameraAssignRID = "ApiCameraAssign";
	public const string CameraAssignAddress = "https://localhost:7278"; // 5245
	public const string CameraAssignPath = "/apiCameraAssign/status";

	public const string IOTRID = "ApiIOT";
	public const string IOTAddress = "https://localhost:7279"; // 5246
	public const string IOTPath = "/apiIOT/status";

	public const string KPIRID = "ApiKPI";
	public const string KPIAddress = "https://localhost:7280"; // 5247
	public const string KPIPath = "/apiKPI/status";

	// 7281 & 5248 is taken by ApiServerReceive

	public const string StationCycleRID = "ApiStationCycle";
	public const string StationCycleAddress = "https://localhost:7282"; // 5249
	public const string StationCyclePath = "/apiStationCycle/status";

	public const string UserRID = "ApiUser";
	public const string UserAddress = "https://localhost:7283"; // 5250
	public const string UserPath = "/apiUser/status";
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
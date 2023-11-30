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
	public const string ADSAddress = "https://localhost:7275";
	public const string ADSPath = "/apiADS/status";

	public const string AlarmRID = "ApiAlarm";
	public const string AlarmAddress = "https://localhost:7276";
	public const string AlarmPath = "/apiAlarm/status";

	public const string CameraRID = "ApiCamera";
	public const string CameraAddress = "https://localhost:7277";
	public const string CameraPath = "/apiCamera/status";

	public const string IOTRID = "ApiIOT";
	public const string IOTAddress = "https://localhost:7278";
	public const string IOTPath = "/apiIOT/status";

	public const string KPIRID = "ApiKPI";
	public const string KPIAddress = "https://localhost:7279";
	public const string KPIPath = "/apiKPI/status";

	// 7280 & 5247 is taken by ApiServerReceive

	public const string StationCycleRID = "ApiStationCycle";
	public const string StationCycleAddress = "https://localhost:7281";
	public const string StationCyclePath = "/apiStationCycle/status";

	public const string UserRID = "ApiUser";
	public const string UserAddress = "https://localhost:7282";
	public const string UserPath = "/apiUser/status";

	public const string VisionRID = "ApiVision";
	public const string VisionAddress = "https://localhost:7283";
	public const string VisionPath = "/apiVision/status";
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
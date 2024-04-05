using Core.Shared.Configuration;

namespace Core.Entities.IOT.Dictionaries;

public static class DeviceRID
{
	public const string Camera1 = "Camera1";
	public const string Camera2 = "Camera2";

	public const string TwinCat = "TwinCat";
}

/// <summary>
/// Provides information about every Api. The Address is given at runtime by loading the base configuration. <see cref="Configuration"/>
/// </summary>
public static class ServerRuleDict
{
	public const string RID = "ServerRule";
}

/// <summary>
/// Provides information about every Api. The Address is given at runtime by loading the base configuration. <see cref="Configuration"/>
/// </summary>
public static class ITApisDict
{
	public const string ADSRID = "ApiADS";
	public static string ADSAddress = string.Empty; // 7275
	public const string ADSPath = "/apiADS/status";

	public const string AlarmRID = "ApiAlarm";
	public static string AlarmAddress = string.Empty; // 7276
	public const string AlarmPath = "/apiAlarm/status";

	public const string CameraRID = "ApiCamera";
	public static string CameraAddress = string.Empty; // 7277
	public const string CameraPath = "/apiCamera/status";

	public const string IOTRID = "ApiIOT";
	public static string IOTAddress = string.Empty; // 7278
	public const string IOTPath = "/apiIOT/status";

	public const string MonitorRID = "ApiMonitor";
	public static string MonitorAddress = string.Empty; // 7280
	public const string MonitorPath = "/apiMonitor/status";

	public const string ServerReceiveRID = "ApiServerReceive";
	public static string ServerReceiveAddress = string.Empty; // 7281
	public const string ServerReceivePath = "/apiServerReceive/status";

	public const string StationCycleRID = "ApiStationCycle";
	public static string StationCycleAddress = string.Empty; // 7282
	public const string StationCyclePath = "/apiStationCycle/status";

	public const string UserRID = "ApiUser";
	public static string UserAddress = string.Empty; // 7283
	public const string UserPath = "/apiUser/status";

	public const string VisionRID = "ApiVision";
	public static string VisionAddress = string.Empty; // 7284
	public const string VisionPath = "/apiVision/status";
}

public static class TriggerSources
{
	public const string Line3 = "Line3";
}

public static class TriggerActivations
{
	public const string AnyEdge = "AnyEdge";
	public const string RisingEdge = "RisingEdge";
}

public static class PixelFormats
{
	public const string BayerRG8 = "BayerRG8";
	public const string RGB8 = "RGB8";
}
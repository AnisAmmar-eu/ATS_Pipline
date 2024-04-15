using Core.Shared.Configuration;
using Core.Shared.Dictionaries;

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

/// <summary>
/// Provides information about 5 Stations. The Address is given at runtime by loading the base configuration. <see cref="Configuration"/>
/// </summary>
public static class ITStationApisDict
{
	public const string StationPath = "/apiStationCycle/oldest";

	public const string Station1RID = "ApiStation1";
	public static string Station1Address = string.Empty;

	public const string Station2RID = "ApiStation2";
	public static string Station2Address = string.Empty;

	public const string Station3RID = "ApiStation3";
	public static string Station3Address = string.Empty;

	public const string Station4RID = "ApiStation4";
	public static string Station4Address = string.Empty;

	public const string Station5RID = "ApiStation5";
	public static string Station5Address = string.Empty;
}

	/*
		Station (S1, S2, S3, S4, S5)
		Sign (All) / Match (S3, S4, S5)
		Cam1 (All), Cam2 (Except S5)
		DX (All), D20 (Except S5)
		Static / Dynamic (Except Match)
	*/
public record SignServiceData(int StationID, string AnodeType);

public static class SignService
{
	public static SignServiceData S1TypeDX = new(1, AnodeTypeDict.DX);
	public static SignServiceData S1TypeD20 = new(1, AnodeTypeDict.D20);
	public static SignServiceData S2TypeDX = new(2, AnodeTypeDict.DX);
	public static SignServiceData S2TypeD20 = new(2, AnodeTypeDict.D20);
	public static SignServiceData S3TypeDX = new(3, AnodeTypeDict.DX);
	public static SignServiceData S3TypeD20 = new(3, AnodeTypeDict.D20);
	public static SignServiceData S4TypeDX = new(4, AnodeTypeDict.DX);
	public static SignServiceData S4TypeD20 = new(4, AnodeTypeDict.D20);
	public static SignServiceData S5TypeDX = new(5, AnodeTypeDict.DX);

	public static List<SignServiceData> list = [
		S1TypeDX,
		S1TypeD20,
		S2TypeDX,
		S2TypeD20,
		S3TypeDX,
		S3TypeD20,
		S4TypeDX,
		S4TypeD20,
		S5TypeDX
	];
}

public record MatchServiceData(
	int StationID,
	string AnodeType,
	string Family,
	int InstanceMatchID);
public static class MatchService
{
	/*
		Gate0 = S1S2
		Gate1 = S3S4
		Gate2 = S5

		Gate1Gate0DX
		Gate1Gate0D20
		Gate2Gate0DX
		Gate2Gate1DX
	*/
	public static MatchServiceData S3Gate1Gate0DXv1 = new(3, "DX", "Gate1Gate0DX", 0);
	public static MatchServiceData S4Gate1Gate0DXv1 = new(4, "DX", "Gate1Gate0DX", 0);
	public static MatchServiceData S3Gate1Gate0DXv2 = new(3, "DX", "Gate1Gate0DX", 4);
	public static MatchServiceData S4Gate1Gate0DXv2 = new(4, "DX", "Gate1Gate0DX", 4);
	public static MatchServiceData S3Gate1Gate0D20v1 = new(3, "D20", "Gate1Gate0D20", 1);
	public static MatchServiceData S4Gate1Gate0D20v1 = new(4, "D20", "Gate1Gate0D20", 1);
	public static MatchServiceData S5Gate2Gate0DXv1 = new(5, "DX", "Gate2Gate0DX", 2);
	public static MatchServiceData S5Gate2Gate1DXv1 = new(5, "DX", "Gate2Gate1DX", 3);

	public static List<MatchServiceData> list = [
		S3Gate1Gate0DXv1,
		S4Gate1Gate0DXv1,
		S3Gate1Gate0DXv2,
		S4Gate1Gate0DXv2,
		S3Gate1Gate0D20v1,
		S4Gate1Gate0D20v1,
		S5Gate2Gate0DXv1,
		S5Gate2Gate1DXv1
	];
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
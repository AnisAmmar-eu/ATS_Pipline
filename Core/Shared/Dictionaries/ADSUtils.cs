using Core.Shared.Services.Background;
using Core.Shared.Services.Notifications;

namespace Core.Shared.Dictionaries;

/// <summary>
/// Provides a set of constant for automaton-related operations such as <see cref="BaseNotification{T,TStruct}"/>
/// or values needed by <see cref="CameraService"/>
/// </summary>
public static class ADSUtils
{
	public const int AdsPort = 851;

	public const string ConnectionPath = "VA_HMI.Connection";

	#region Camera

	public const string GlobalRID = "VA_STATION.CycleRID";
	public const string GlobalAnodeType = "VA_STATION.Anode.AnodeType";
	public const string HasPlug = "VA_STATION.Plug";
	public const string PictureCountCam1 = "VA_STATION.PictureCounter_Cam01";
	public const string PictureCountCam2 = "VA_STATION.PictureCounter_Cam02";

	public const string IsHole1 = "VA_STATION.IsHole1";

	#endregion Camera

	#region AlarmNotification

	public const string AlarmNewMsg = "VA_ALM.MsgNew"; // Notifies when there is a new message.
	public const string AlarmToRead = "VA_ALM.Out"; // Data zone and root of the structure.

	#endregion AlarmNotification

	#region AlarmRT

	public const string AlarmList = "VA_ALM.List";
	public const string SyncTime = "VA_IT.PLC_TT_SER";
	public const string DiskSpace = "VA_IT.DISK_SPACE";

	#endregion AlarmRT

	#region MetaDataNotification

	public const string MetaDataNewMsg = "VA_P05.MsgNew";
	public const string MetaDataToRead = "VA_P05.Out";

	#endregion MetaDataNotification

	#region InFurnaceNotification

	public const string InFurnaceNewMsg = "VA_P21.MsgNew";
	public const string InFurnaceToRead = "VA_P21.Out";

	#endregion InFurnaceNotification

	#region OutFurnaceNotification

	public const string OutFurnaceNewMsg = "VA_P22.MsgNew";
	public const string OutFurnaceToRead = "VA_P22.Out";

	#endregion OutFurnaceNotification

}
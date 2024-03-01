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
	public const string PictureCountCam1 = "VA_STATION.PictureCounter_Cam01";
	public const string PictureCountCam2 = "VA_STATION.PictureCounter_Cam02";

	public const string IsHole1 = "VA_STATION.IsHole1";

	#endregion

	#region AlarmNotification

	public const string AlarmNewMsg = "VA_ALM.MsgNew"; // Notifies when there is a new message.
	public const string AlarmToRead = "VA_ALM.Out"; // Data zone and root of the structure.

	#endregion

	#region AlarmRT

	public const string AlarmList = "VA_ALM.List";
	public const string AlarmTime = "VA_IT.AlarmTime";

	#endregion

	#region ShootingNotification

	public const string ShootingNewMsg = "VA_P05.MsgNew";
	public const string ShootingToRead = "VA_P05.Out";

	#endregion

	#region InFurnaceNotification

	public const string InFurnaceNewMsg = "VA_P21.MsgNew";
	public const string InFurnaceToRead = "VA_P21.Out";

	#endregion

	#region OutFurnaceNotification

	public const string OutFurnaceNewMsg = "VA_P22.MsgNew";
	public const string OutFurnaceToRead = "VA_P22.Out";

	#endregion

}
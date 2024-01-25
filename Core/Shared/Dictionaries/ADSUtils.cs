namespace Core.Shared.Dictionaries;

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

	public const string AlarmRemove = "VA_ALM.remove"; // Delete once consumed.
	public const string AlarmNewMsg = "VA_ALM.msgNew"; // Notifies when there is a new message.
	public const string AlarmToRead = "VA_ALM.oldEntry"; // Data zone and root of the structure.

	#endregion

	#region AnnouncementNotification

	public const string AnnouncementRemove = "VA_P01.SQL_FIFO.remove";
	public const string AnnouncementNewMsg = "VA_P01.SQL_FIFO.msgNew";
	public const string AnnouncementToRead = "VA_P01.SQL_FIFO.OldEntry";

	#endregion

	#region ShootingNotification

	public const string ShootingRemove = "VA_P05.SQL_FIFO.remove";
	public const string ShootingNewMsg = "VA_P05.SQL_FIFO.msgNew";
	public const string ShootingToRead = "VA_P05.SQL_FIFO.OldEntry";

	#endregion

	#region InFurnaceNotification

	public const string InFurnaceRemove = "VA_P21.SQL_FIFO.remove";
	public const string InFurnaceNewMsg = "VA_P21.SQL_FIFO.msgNew";
	public const string InFurnaceToRead = "VA_P21.SQL_FIFO.OldEntry";

	#endregion

	#region OutFurnaceNotification

	public const string OutFurnaceRemove = "VA_P22.SQL_FIFO.remove";
	public const string OutFurnaceNewMsg = "VA_P22.SQL_FIFO.msgNew";
	public const string OutFurnaceToRead = "VA_P22.SQL_FIFO.OldEntry";

	#endregion

}
namespace Core.Shared.Dictionaries;

public static class ADSUtils
{
	public const int AdsPort = 851;

	public const string ConnectionPath = "VA_HMI.Connection";
	public const string GlobalRIDForCamera = "VA_FAT.CycleRID.StationID";

	public const string MeasurementVariable = "VA_PXX.P04_In";

	public const string CloseCycle = "";

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

	// Todo : Remove this if DetectionNotification is not used.
	#region DetectionNotification

	public const string DetectionRemove = "VA_P02.SQL_FIFO.remove";
	public const string DetectionNewMsg = "VA_P02.SQL_FIFO.msgNew";
	public const string DetectionToRead = "VA_P02.SQL_FIFO.OldEntry.Out";

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

	#region ShootingNotification

	public const string ShootingAcquitMsg = "";
	public const string ShootingNewMsg = "";
	public const string ShootingToRead = "";

	#endregion
}
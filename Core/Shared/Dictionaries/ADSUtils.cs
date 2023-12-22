namespace Core.Shared.Dictionaries;

public static class ADSUtils
{
	public const int AdsPort = 851;

	public const string GlobalRIDForCamera = "VA_FAT.CycleRID.SN";

	public const string MeasurementVariable = "VA_PXX.P04_In";

	public const string CloseCycle = "";

	#region AlarmNotification

	public const string AlarmRemove = "VA_ALM.remove"; // pour suprimer une fois consomé
	public const string AlarmNewMsg = "VA_ALM.msgNew"; // pour surveiller qu'il y un nouveau msg
	public const string AlarmToRead = "VA_ALM.oldEntry"; // zone de lecture et racibe de la structure

	#endregion

	#region AnnouncementNotification

	public const string AnnouncementRemove = "VA_PXX.P01_SQL_FIFO.remove";
	public const string AnnouncementNewMsg = "VA_PXX.P01_SQL_FIFO.msgNew";
	public const string AnnouncementToRead = "VA_PXX.P01_SQL_FIFO.OldEntry.Out";

	#endregion

	#region DetectionNotification

	public const string DetectionRemove = "VA_PXX.P02_SQL_FIFO.remove";
	public const string DetectionNewMsg = "VA_PXX.P02_SQL_FIFO.msgNew";
	public const string DetectionToRead = "VA_PXX.P02_SQL_FIFO.OldEntry.Out";

	#endregion

	#region InFurnaceNotification

	public const string InFurnaceRemove = "VA_PXX.P21_SQL_FIFO.remove";
	public const string InFurnaceNewMsg = "VA_PXX.P21_SQL_FIFO.msgNew";
	public const string InFurnaceToRead = "VA_PXX.P21_SQL_FIFO.OldEntry.Out";

	#endregion

	#region OutFurnaceNotification

	public const string OutFurnaceRemove = "VA_PXX.P22_SQL_FIFO.remove";
	public const string OutFurnaceNewMsg = "VA_PXX.P22_SQL_FIFO.msgNew";
	public const string OutFurnaceToRead = "VA_PXX.P22_SQL_FIFO.OldEntry.Out";

	#endregion

	#region ShootingNotification

	public const string ShootingAcquitMsg = "";
	public const string ShootingNewMsg = "";
	public const string ShootingToRead = "";

	#endregion
}
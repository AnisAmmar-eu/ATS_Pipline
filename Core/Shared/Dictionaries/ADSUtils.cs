namespace Core.Shared.Dictionaries;

public class ADSUtils
{
	// AlarmNotification
	public const string AlarmAcquitMsg = "VA_ALM.msgAcquit";
	public const string AlarmNewMsg = "VA_ALM.msgNew";
	public const string AlarmToRead = "VA_ALM.oldEntry";

	// AnnouncementNotification
	public const string AnnouncementRemove = "VA_PXX.P01_SQL_FIFO.remove";
	public const string AnnouncementNewMsg = "VA_PXX.P01_SQL_FIFO.msgNew";
	public const string AnnouncementToRead = "VA_PXX.P01_SQL_FIFO.OldEntry.Out";

	// DetectionNotification
	public const string DetectionRemove = "VA_PXX.P02_SQL_FIFO.remove";
	public const string DetectionNewMsg = "VA_PXX.P02_SQL_FIFO.msgNew";
	public const string DetectionToRead = "VA_PXX.P02_SQL_FIFO.OldEntry.Out";

	// InFurnaceNotification
	public const string InFurnaceRemove = "VA_PXX.P21_SQL_FIFO.remove";
	public const string InFurnaceNewMsg = "VA_PXX.P21_SQL_FIFO.msgNew";
	public const string InFurnaceToRead = "VA_PXX.P21_SQL_FIFO.OldEntry.Out";

	// OutFurnaceNotification
	public const string OutFurnaceRemove = "VA_PXX.P22_SQL_FIFO.remove";
	public const string OutFurnaceNewMsg = "VA_PXX.P22_SQL_FIFO.msgNew";
	public const string OutFurnaceToRead = "VA_PXX.P22_SQL_FIFO.OldEntry.Out";

	// ShootingNotification
	public const string ShootingAcquitMsg = "";
	public const string ShootingNewMsg = "";
	public const string ShootingToRead = "";
	public static int AdsPort = 851;

	// MeasureNotification	
	public static string MeasurementVariable = "VA_PXX.P04_In";
}
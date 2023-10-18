namespace ApiADS;

public class Utils
{
	// AlarmNotification
	public const string AlarmAcquitMsg = "VA_ALM.msgAcquit";
	public const string AlarmNewMsg = "VA_ALM.msgNew";
	public const string AlarmToRead = "VA_ALM.oldEntry.Out";
	
	// AnnouncementNotification
	public const string AnnouncementAcquitMsg = "VA_PXX.P01_SQL_FIFO.msgAcquit";
	public const string AnnouncementNewMsg = "VA_PXX.P01_SQL_FIFO.msgNew";
	public const string AnnouncementToRead = "VA_PXX.P01_SQL_FIFO.OldEntry.Out";

	// DetectionNotification
	public const string DetectionAcquitMsg = "VA_PXX.P02_SQL_FIFO.msgAcquit";
	public const string DetectionNewMsg = "VA_PXX.P02_SQL_FIFO.msgNew";
	public const string DetectionToRead = "VA_PXX.P02_SQL_FIFO.OldEntry.Out";

	// InFurnaceNotification
	public const string InFurnaceAcquitMsg = "VA_PXX.P21_SQL_FIFO.msgAcquit";
	public const string InFurnaceNewMsg = "VA_PXX.P21_SQL_FIFO.msgNew";
	public const string InFurnaceToRead = "VA_PXX.P21_SQL_FIFO.OldEntry.Out";

	// OutFurnaceNotification
	public const string OutFurnaceAcquitMsg = "VA_PXX.P22_SQL_FIFO.msgAcquit";
	public const string OutFurnaceNewMsg = "VA_PXX.P22_SQL_FIFO.msgNew";
	public const string OutFurnaceToRead = "VA_PXX.P22_SQL_FIFO.OldEntry.Out";

	// ShootingNotification
	public const string ShootingAcquitMsg = "";
	public const string ShootingNewMsg = "";
	public const string ShootingToRead = "";

	// AcquitMsg
	public const int ErrorWhileReading = 0;
	public const int IsReading = 1;
	public const int FinishedReading = 2;

	// NewMsg
	public const int NoMessage = 0;
	public const int HasNewMsg = 2;
}
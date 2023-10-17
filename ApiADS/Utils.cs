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
	public const string DetectionAcquitMsg = "VA_PXX.P01_SQL_FIFO.msgAcquit";
	public const string DetectionNewMsg = "VA_PXX.P01_SQL_FIFO.msgNew";
	public const string DetectionToRead = "VA_PXX.P01_SQL_FIFO.OldEntry.Out";

	// InFurnaceNotification
	public const string InFurnaceAcquitMsg = "";
	public const string InFurnaceNewMsg = "";
	public const string InFurnaceToRead = "";

	// OutFurnaceNotification
	public const string OutFurnaceAcquitMsg = "";
	public const string OutFurnaceNewMsg = "";
	public const string OutFurnaceToRead = "";

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
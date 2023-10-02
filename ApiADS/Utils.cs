namespace ApiADS;

public class Utils
{
	// AlarmNotification
	public const string AlarmAcquitMsg = "VA_ALM.msgAcquit";
	public const string AlarmNewMsg = "VA_ALM.msgNew";
	public const string AlarmToRead = "VA_ALM.oldEntry";

	// DetectionNotification
	public const string DetectionAcquitMsg = "";
	public const string DetectionNewMsg = "";
	public const string DetectionToRead = "";

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
namespace ApiADS;

public class Utils
{
	public const string AcquitMsg = "VA_ALM.msgAcquit";
	public const string NewMsg = "VA_ALM.msgNew";
	public const string ToRead = "VA_ALM.oldEntry";

	//AcquitMsg
	public const int ErrorWhileReading = 0;
	public const int IsReading = 1;
	public const int FinishedReading = 2;
	
	// NewMsg
	public const int NoMessage = 0;
	public const int HasNewMsg = 2;
}
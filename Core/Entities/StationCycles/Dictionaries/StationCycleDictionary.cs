namespace Core.Entities.StationCycles.Dictionaries;

public class CycleStatus
{
	public static string Initialized = "Initialised";
	public static string Running = "Running";
	public static string Finished = "Finished";
	public static string Closed = "Closed";
}

public class CycleAdsUtils
{
	public static int AdsPort = 851;
	public static string MeasurementVariable = "VA_PXX.P04_In";
}
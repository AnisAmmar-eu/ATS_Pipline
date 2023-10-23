namespace Core.Entities.StationCycles.Dictionaries;

public class CycleStatus
{
	public static string Initialized = "Initialised";
	public static string Running = "Running";
	public static string Finished = "Finished";
	public static string Closed = "Closed";
}

public static class CycleTypes
{
	public const string S1S2 = "S1S2Cycle";
	public const string S3S4 = "S3S4Cycle";
	public const string S5 = "S5Cycle";
}
namespace Core.Entities.IOT.Dictionaries;

public class IOTTagNames
{
	public static string CheckConnectionName = "__Connection";
}

public class IOTTagType
{
	public static string String = "string";
	public static string Int = "int";
	public static string Bool = "bool";
}

public static class DeviceRID
{
	public static string Camera1 = "Camera1";
	public static string Camera2 = "Camera2";
}

public static class TriggerSources
{
	public const string Line3 = "Line3";
}

public static class TriggerActivations
{
	public const string AnyEdge = "AnyEdge";
}

public static class PixelFormats
{
	public const string BayerRG8 = "BayerRG8";
}
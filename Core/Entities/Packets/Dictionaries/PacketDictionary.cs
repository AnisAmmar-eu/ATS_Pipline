namespace Core.Entities.Packets.Dictionaries;

public enum PacketStatus
{
	Initialized = 1,
	Completed = 3,
	Sent = 4,
}

public static class PacketTypes
{
	public const string AlarmList = "AlarmList";
	public const string Shooting = "Shooting";
	public const string MetaData = "MetaData";
	public const string InFurnace = "InFurnace";
	public const string OutFurnace = "OutFurnace";
}

public static class ShootingUtils
{
	public const string TestFilename = "TestImage.jpeg";

	public const string CameraTest1 = @"C:\.ats\CameraTest1\";
	public const string CameraTest2 = @"C:\.ats\CameraTest2\";
	public const string Archive1 = @"C:\.ats\Archives1\";
	public const string Archive2 = @"C:\.ats\Archives2\";
}
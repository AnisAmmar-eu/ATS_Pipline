namespace Core.Entities.Packets.Dictionaries;

public enum PacketStatus
{
	Initialized = 1,
	Running = 2,
	Completed = 3,
	Sent = 4,
}

public static class PacketType
{
	public const string Alarm = "Alarm";
	public const string Announcement = "Annnouncement";
	public const string S1S2Announcement = "S1S2announcement";
	public const string Detection = "Detection";
	public const string Shooting = "Shooting";
	public const string InFurnace = "InFurnace";
	public const string OutFurnace = "OutFurnace";
}

public static class ShootingUtils
{
	public const string TestFilename = "TestImage.jpg";

	public const string CameraTest1 = @"C:\.ats\CameraTest1\";
	public const string CameraTest2 = @"C:\.ats\CameraTest2\";
	public const string Archive1 = @"C:\.ats\Archives1\";
	public const string Archive2 = @"C:\.ats\Archives2\";
}
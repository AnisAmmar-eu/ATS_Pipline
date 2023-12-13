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
	public const string Alarm = "ALARM";
	public const string Announcement = "ANNOUNCEMENT";
	public const string S1S2Announcement = "S1S2ANNOUNCEMENT";
	public const string Detection = "DETECTION";
	public const string Shooting = "SHOOTING";
	public const string InFurnace = "INFURNACE";
	public const string OutFurnace = "OUTFURNACE";
}

public static class ShootingUtils
{
	public const string TestFilename = "TestImage.jpg";

	public const string Camera1 = @"C:\.ats\ApiCamera\Camera1\";
	public const string Camera2 = @"C:\.ats\ApiCamera\Camera2\";
	public const string CameraTest1 = @"C:\.ats\CameraTest1\";
	public const string CameraTest2 = @"C:\.ats\CameraTest2\";
	public const string Archive1 = @"C:\.ats\Archives1\";
	public const string Archive2 = @"C:\.ats\Archives2\";
}
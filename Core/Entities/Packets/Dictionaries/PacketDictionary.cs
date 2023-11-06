namespace Core.Entities.Packets.Dictionaries;

public class PacketStatus
{
	public const string Initialized = "Initialized";
	public const string Running = "Running";
	public const string Completed = "Completed";
	public const string Sent = "Sent";
}

public class PacketType
{
	public const string Alarm = "ALARM";
	public const string Announcement = "ANNOUNCEMENT";
	public const string S1S2Announcement = "S1S2ANNOUNCEMENT";
	public const string Detection = "DETECTION";
	public const string Shooting = "SHOOTING";
	public const string InFurnace = "INFURNACE";
	public const string OutFurnace = "OUTFURNACE";
}

public class ShootingUtils
{
	// Filename
	public const string TestFilename = "TestImage.jpg";

	// Directories
	public const string Camera1 = @"..\ApiCamera\Camera1\";
	public const string Camera2 = @"..\ApiCamera\Camera2\";
	public const string CameraTest1 = @"CameraTest1\";
	public const string CameraTest2 = @"CameraTest2\";
	public const string Archive1 = @"Archives1\";
	public const string Archive2 = @"Archives2\";
}
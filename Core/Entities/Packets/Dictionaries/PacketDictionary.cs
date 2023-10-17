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
	public const string Detection = "DETECTION";
	public const string Shooting = "SHOOTING";
	public const string Furnace = "FURNACE";
}

public class ShootingFolders
{
	public const string Camera1 = @"..\ApiCamera\Camera1\";
	public const string Camera2 = @"..\ApiCamera\Camera2\";
	public const string Archive1 = @"Archives1\";
	public const string Archive2 = @"Archives2\";
}
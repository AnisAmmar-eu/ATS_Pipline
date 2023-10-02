namespace Core.Entities.Packets.Dictionary;

public enum PacketStatus
{
	Initialised,
	Running,
	Completed,
	Sent
}

public enum AnodeType
{
	D20,
	DX
}

public class PacketType
{
	public const string ALARM = "ALARM";
	public const string ANNOUNCEMENT = "ANNOUNCMENT";
	public const string DETECTION = "DETECTION";
	public const string SHOOTING = "SHOOTING";
	public const string FURNACE = "FURNACE";
}
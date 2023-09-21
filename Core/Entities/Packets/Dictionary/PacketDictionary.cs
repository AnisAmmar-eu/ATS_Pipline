namespace Core.Entities.Packets.Dictionary;

public enum PacketStatus
{
	Initialised,
	Running,
	Completed,
	Sent,
}

public class PacketType
{
	public const string ALARM = "ALARM";
	public const string DETECTION = "DETECTION";
}
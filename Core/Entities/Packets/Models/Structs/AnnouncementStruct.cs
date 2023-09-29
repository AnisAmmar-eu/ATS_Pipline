namespace Core.Entities.Packets.Models.Structs;

public struct AnnouncementStruct
{
	public uint CycleStationRID;
	public uint AnnouncementID; // TODO ? + 8*UINT?
	public uint AnodeType;
	public uint Double; // TODO ?
	
	// TODO Determine how to handle those, null in non valid stations?
	// TODO Or maybe check in which station we are, account for these in consequence.
	public uint SerialNumber; // TODO STR or 8*UINT
	public uint TrolleyNumber;
}
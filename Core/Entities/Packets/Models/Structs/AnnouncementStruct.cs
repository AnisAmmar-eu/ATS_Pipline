using Core.Entities.Packets.Models.DB.Announcements;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct AnnouncementStruct : IBaseADS<Announcement, AnnouncementStruct>
{
	public uint CycleStationRID;
	public uint AnnouncementID; // TODO ? + 8*UINT?
	public uint AnodeType;
	public uint Double; // TODO ?
	
	// TODO Determine how to handle those, null in non valid stations?
	// TODO Or maybe check in which station we are, account for these in consequence.
	public uint SerialNumber; // TODO STR or 8*UINT
	public uint TrolleyNumber;
	public Announcement ToModel()
	{
		return new Announcement(this);
	}
}
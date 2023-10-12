using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct AnnouncementStruct : IBaseADS<Packet, AnnouncementStruct>
{
	public RIDStruct StationCycleRID;
	public uint AnnounceID;
	public uint AnodeType;

	// TODO Determine how to handle those, null in non valid stations?
	// TODO Or maybe check in which station we are, account for these in consequence.
	public string SerialNumber;
	public uint TrolleyNumber;

	public Packet ToModel()
	{
		return new Announcement(this);
	}
}
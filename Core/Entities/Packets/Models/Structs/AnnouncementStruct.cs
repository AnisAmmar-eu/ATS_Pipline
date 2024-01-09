using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct AnnouncementStruct : IBaseADS<Packet>
{
	// Missing AnnounceID?
	public RIDStruct CycleRID;
	public int Status;
	public TimestampStruct TS;
	// Unused => Put in db
	public int SyncIndex;
	// Unused => same
	public int Double;
	public int AnodeType;

	public RIDStruct SN;
	public int TrolleyNumber;

	public Packet ToModel()
	{
		return (Station.Type == StationType.S1S2) ? new S1S2Announcement(this) : new Announcement(this);
	}
}
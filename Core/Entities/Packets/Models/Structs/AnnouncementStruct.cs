using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct AnnouncementStruct : IBaseADS<Packet, AnnouncementStruct>
{
	public RIDStruct RID;
	public RIDStruct AnnounceID;
	public ushort AnodeType;

	// TODO Might not be present if not in S1/S2
	public ushort TrolleyNumber;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string SerialNumber;

	public Packet ToModel()
	{
		return Station.Type == StationType.S1S2 ? new S1S2Announcement(this) : new Announcement(this);
	}
}
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Announcements;

public partial class DTOAnnouncement : DTOPacket, IDTO<Announcement, DTOAnnouncement>
{
	public DTOAnnouncement() : base()
	{
		Type = PacketType.Announcement;
	}

	public DTOAnnouncement(Announcement announcement) : base(announcement)
	{
		Type = PacketType.Announcement;
		AnodeType = announcement.AnodeType;
		AnnounceID = announcement.AnnounceID;
	}

	public override Announcement ToModel()
	{
		return new Announcement(this);
	}
}
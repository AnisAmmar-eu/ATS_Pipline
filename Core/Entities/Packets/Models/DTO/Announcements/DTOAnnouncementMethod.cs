using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Announcements;

namespace Core.Entities.Packets.Models.DTO.Announcements;

public partial class DTOAnnouncement
{
	public DTOAnnouncement()
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
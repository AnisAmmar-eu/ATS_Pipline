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
		SyncIndex = announcement.SyncIndex;
		IsDouble = announcement.IsDouble;
	}

	public override Announcement ToModel()
	{
		return new(this);
	}
}
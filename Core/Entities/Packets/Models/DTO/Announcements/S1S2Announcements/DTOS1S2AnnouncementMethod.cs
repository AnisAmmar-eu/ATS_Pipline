using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;

namespace Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;

public partial class DTOS1S2Announcement
{
	public DTOS1S2Announcement()
	{
		Type = PacketType.S1S2Announcement;
	}

	public DTOS1S2Announcement(S1S2Announcement s1S2Announcement) : base(s1S2Announcement)
	{
		Type = PacketType.S1S2Announcement;
		SerialNumber = s1S2Announcement.SerialNumber;
		TrolleyNumber = s1S2Announcement.TrolleyNumber;
	}

	public override S1S2Announcement ToModel()
	{
		return new S1S2Announcement(this);
	}
}
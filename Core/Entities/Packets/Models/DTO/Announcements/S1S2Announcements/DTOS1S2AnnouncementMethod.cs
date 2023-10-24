using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;

public partial class DTOS1S2Announcement : DTOAnnouncement, IDTO<S1S2Announcement, DTOS1S2Announcement>
{
	public DTOS1S2Announcement(S1S2Announcement s1S2Announcement) : base(s1S2Announcement)
	{
		SerialNumber = s1S2Announcement.SerialNumber;
		TrolleyNumber = s1S2Announcement.TrolleyNumber;
	}

	public override S1S2Announcement ToModel()
	{
		return new S1S2Announcement(this);
	}
}
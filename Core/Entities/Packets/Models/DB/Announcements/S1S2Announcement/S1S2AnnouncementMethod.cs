using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;

public partial class S1S2Announcement : Announcement, IBaseEntity<S1S2Announcement, DTOS1S2Announcement>
{
	public override DTOS1S2Announcement ToDTO(string? languageRID = null)
	{
		return new DTOS1S2Announcement(this);
	}
}
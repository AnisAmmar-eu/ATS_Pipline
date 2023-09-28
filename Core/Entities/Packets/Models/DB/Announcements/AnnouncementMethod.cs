using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements;

public partial class Announcement : Packet, IBaseEntity<Announcement, DTOAnnouncement>
{
	public override DTOAnnouncement ToDTO(string? languageRID = null)
	{
		return new DTOAnnouncement(this);
	}
}
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements;

public partial class Announcement : Packet, IBaseEntity<Announcement, DTOAnnouncement>
{
	public string AnodeType { get; set; } = string.Empty;
	public string AnnounceID { get; set; } = string.Empty;
}
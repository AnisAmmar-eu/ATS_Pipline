using Core.Entities.Packets.Models.DB.Announcements;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Announcements;

public partial class DTOAnnouncement : DTOPacket, IDTO<Announcement, DTOAnnouncement>
{
	public string AnodeType { get; set; }
}
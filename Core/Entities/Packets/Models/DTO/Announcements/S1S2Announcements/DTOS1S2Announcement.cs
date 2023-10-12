using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;

public partial class DTOS1S2Announcement : DTOAnnouncement, IDTO<S1S2Announcement, DTOS1S2Announcement>
{
	public string SerialNumber { get; set; }
	public int TrolleyNumber { get; set; }
}
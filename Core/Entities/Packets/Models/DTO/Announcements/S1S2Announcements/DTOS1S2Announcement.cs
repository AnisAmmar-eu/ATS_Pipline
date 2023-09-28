using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;

public partial class DTOS1S2Announcement : DTOAnnouncement, IDTO<S1S2Announcement, DTOS1S2Announcement>
{
	public int SerialNumber;
	public int TrolleyNumber;
}
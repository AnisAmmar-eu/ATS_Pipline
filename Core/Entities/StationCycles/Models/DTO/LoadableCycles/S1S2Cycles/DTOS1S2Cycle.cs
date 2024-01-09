using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;

public partial class DTOS1S2Cycle : DTOLoadableCycle, IDTO<S1S2Cycle, DTOS1S2Cycle>
{
	new public DTOS1S2Announcement? AnnouncementPacket { get; set; }
}
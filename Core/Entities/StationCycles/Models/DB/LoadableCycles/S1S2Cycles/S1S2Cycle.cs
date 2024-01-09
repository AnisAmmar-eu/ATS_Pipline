using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;

public partial class S1S2Cycle : LoadableCycle, IBaseEntity<S1S2Cycle, DTOS1S2Cycle>
{
	new public S1S2Announcement? AnnouncementPacket { get; set; }
}
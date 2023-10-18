using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.S1S2Cycles;

public partial class DTOS1S2Cycle : DTOStationCycle, IDTO<S1S2Cycle, DTOS1S2Cycle>
{
	public new DTOS1S2Announcement? AnnouncementPacket { get; set; }
}
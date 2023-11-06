using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.S1S2Cycles;

public partial class DTOS1S2Cycle : DTOStationCycle, IDTO<S1S2Cycle, DTOS1S2Cycle>
{
	public DTOS1S2Cycle()
	{
		CycleType = CycleTypes.S1S2;
	}

	public DTOS1S2Cycle(S1S2Cycle s1S2Cycle) : base(s1S2Cycle)
	{
		CycleType = CycleTypes.S1S2;
		// Query will set the announce in the parent attribute instead of the overriden one.
		if (s1S2Cycle.AnnouncementPacket == null && base.AnnouncementPacket != null)
			AnnouncementPacket = (DTOS1S2Announcement?)base.AnnouncementPacket;
		else AnnouncementPacket = s1S2Cycle.AnnouncementPacket?.ToDTO();
	}

	public override S1S2Cycle ToModel()
	{
		return new S1S2Cycle(this);
	}
}
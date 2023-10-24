using Core.Entities.StationCycles.Models.DTO.S1S2Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S1S2Cycles;

public partial class S1S2Cycle : StationCycle, IBaseEntity<S1S2Cycle, DTOS1S2Cycle>
{
	public S1S2Cycle()
	{
	}

	public S1S2Cycle(DTOS1S2Cycle dtoS1S2Cycle)
	{
		AnnouncementPacket = dtoS1S2Cycle.AnnouncementPacket?.ToModel();
	}

	public override DTOS1S2Cycle ToDTO()
	{
		return new DTOS1S2Cycle(this);
	}
}
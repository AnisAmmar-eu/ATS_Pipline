using Core.Entities.StationCycles.Interfaces;
using Core.Entities.StationCycles.Models.DTO.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S5Cycles;

public partial class S5Cycle : StationCycle, IBaseEntity<S5Cycle, DTOS5Cycle>, IMatchableCycle
{
	public S5Cycle()
	{
	}

	public S5Cycle(DTOS5Cycle dtoS5Cycle) : base(dtoS5Cycle)
	{
		MatchingCamera1 = dtoS5Cycle.MatchingCamera1;
		MatchingCamera2 = dtoS5Cycle.MatchingCamera2;
	}

	public override DTOS5Cycle ToDTO()
	{
		return new DTOS5Cycle(this);
	}
}
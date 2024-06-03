using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Mapster;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;

public partial class S5Cycle
{
	public S5Cycle()
	{
	}

	public override DTOS5Cycle ToDTO() => this.Adapt<DTOS5Cycle>();
}
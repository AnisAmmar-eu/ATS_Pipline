using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Mapster;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;

public partial class DTOS5Cycle
{
	public DTOS5Cycle()
	{
		CycleType = CycleTypes.S5;
	}

	public override S5Cycle ToModel() => this.Adapt<S5Cycle>();
}
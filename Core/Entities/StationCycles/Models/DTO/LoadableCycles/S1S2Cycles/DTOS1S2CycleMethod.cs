using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Mapster;

namespace Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;

public partial class DTOS1S2Cycle
{
	public DTOS1S2Cycle()
	{
		CycleType = CycleTypes.S1S2;
	}

	public override S1S2Cycle ToModel() => this.Adapt<S1S2Cycle>();
}
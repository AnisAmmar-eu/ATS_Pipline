using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;

namespace Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;

public partial class DTOS1S2Cycle
{
	public DTOS1S2Cycle()
	{
		CycleType = CycleTypes.S1S2;
	}

	public DTOS1S2Cycle(S1S2Cycle s1S2Cycle) : base(s1S2Cycle)
	{
		CycleType = CycleTypes.S1S2;
    }

	public override S1S2Cycle ToModel()
	{
		return new(this);
	}
}
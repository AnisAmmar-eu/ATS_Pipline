using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;

public partial class DTOS5Cycle
{
	public DTOS5Cycle()
	{
		CycleType = CycleTypes.S5;
	}

	public DTOS5Cycle(S5Cycle s5Cycle) : base(s5Cycle)
	{
		CycleType = CycleTypes.S5;
		MatchingCamera1 = s5Cycle.MatchingCamera1;
		MatchingCamera2 = s5Cycle.MatchingCamera2;
	}

	public override S5Cycle ToModel()
	{
		return new(this);
	}
}
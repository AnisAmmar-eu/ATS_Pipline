using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;

namespace Core.Entities.StationCycles.Models.DB.MatchingCycles.S5Cycles;

public partial class S5Cycle
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
		return new(this);
	}
}
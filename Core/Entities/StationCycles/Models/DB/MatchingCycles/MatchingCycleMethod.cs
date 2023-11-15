using Core.Entities.StationCycles.Models.DTO.MatchingCycles;

namespace Core.Entities.StationCycles.Models.DB.MatchingCycles;

public partial class MatchingCycle
{
	public MatchingCycle()
	{
	}

	public MatchingCycle(DTOMatchingCycle dtoMatchingCycle) : base(dtoMatchingCycle)
	{
		MatchingCamera1 = dtoMatchingCycle.MatchingCamera1;
		MatchingCamera2 = dtoMatchingCycle.MatchingCamera2;
	}

	public override DTOMatchingCycle ToDTO()
	{
		return new DTOMatchingCycle(this);
	}
}
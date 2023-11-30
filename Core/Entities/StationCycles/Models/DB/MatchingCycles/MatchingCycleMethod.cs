using Core.Entities.StationCycles.Models.DTO.MatchingCycles;

namespace Core.Entities.StationCycles.Models.DB.MatchingCycles;

public abstract partial class MatchingCycle
{
	protected MatchingCycle()
	{
	}

	protected MatchingCycle(DTOMatchingCycle dtoMatchingCycle) : base(dtoMatchingCycle)
	{
		MatchingCamera1 = dtoMatchingCycle.MatchingCamera1;
		MatchingCamera2 = dtoMatchingCycle.MatchingCamera2;
	}

	public override DTOMatchingCycle ToDTO()
	{
		return new(this);
	}
}
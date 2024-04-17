using Core.Entities.StationCycles.Models.DTO.MatchingCycles;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles;

public abstract partial class MatchableCycle
{
	protected MatchableCycle()
	{
	}

	protected MatchableCycle(DTOMatchingCycle dtoMatchingCycle) : base(dtoMatchingCycle)
	{
		MatchingCamera1 = dtoMatchingCycle.MatchingCamera1;
		MatchingCamera2 = dtoMatchingCycle.MatchingCamera2;
	}

	public override DTOMatchingCycle ToDTO() => new(this);
}
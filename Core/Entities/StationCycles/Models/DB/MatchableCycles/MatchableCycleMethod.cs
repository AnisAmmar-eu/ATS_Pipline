using Core.Entities.StationCycles.Models.DTO.MatchingCycles;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles;

public abstract partial class MatchableCycle
{
	protected MatchableCycle()
	{
	}

	public override DTOMatchingCycle ToDTO() => new(this);
}
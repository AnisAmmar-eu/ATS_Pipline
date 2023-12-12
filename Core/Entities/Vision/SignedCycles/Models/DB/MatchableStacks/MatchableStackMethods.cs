using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;

namespace Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;

public partial class MatchableStack
{
	public MatchableStack()
	{
	}

	public MatchableStack(DTOMatchableStack dtoMatchableStack) : base(dtoMatchableStack)
	{
		MatchableCycleID = dtoMatchableStack.MatchableCycleID;
		if (dtoMatchableStack.MatchableCycle is not null)
			MatchableCycle = dtoMatchableStack.MatchableCycle;
	}

	public override DTOMatchableStack ToDTO()
	{
		return new(this);
	}
}
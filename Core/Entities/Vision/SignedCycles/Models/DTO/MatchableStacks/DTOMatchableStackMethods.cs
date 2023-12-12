using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;

public partial class DTOMatchableStack
{
	public DTOMatchableStack()
	{
	}

	public DTOMatchableStack(MatchableStack matchableStack) : base(matchableStack)
	{
		MatchableCycleID = matchableStack.MatchableCycleID;
		MatchableCycle = matchableStack.MatchableCycle;
	}

	public override MatchableStack ToModel()
	{
		return new(this);
	}
}
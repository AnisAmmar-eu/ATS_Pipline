using Core.Entities.Vision.SignedCycles.Models.DTO.ToMatchs;

namespace Core.Entities.Vision.SignedCycles.Models.DB.ToMatchs;

public partial class ToMatch
{
	public ToMatch()
	{
	}

	public ToMatch(DTOToMatch dtoMatchableStack) : base(dtoMatchableStack)
	{
		MatchableCycleID = dtoMatchableStack.MatchableCycleID;
		if (dtoMatchableStack.MatchableCycle is not null)
			MatchableCycle = dtoMatchableStack.MatchableCycle;
	}

	public override DTOToMatch ToDTO()
	{
		return new(this);
	}
}
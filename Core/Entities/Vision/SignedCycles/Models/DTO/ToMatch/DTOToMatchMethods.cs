using Core.Entities.Vision.SignedCycles.Models.DB.ToMatchs;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.ToMatchs;

public partial class DTOToMatch
{
	public DTOToMatch()
	{
	}

	public DTOToMatch(ToMatch matchableStack) : base(matchableStack)
	{
		MatchableCycleID = matchableStack.MatchableCycleID;
		MatchableCycle = matchableStack.MatchableCycle;
	}

	public override ToMatch ToModel()
	{
		return new(this);
	}
}
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;

public partial class DTOToMatch
{
	public DTOToMatch()
	{
	}

	public DTOToMatch(ToMatch toMatch) : base(toMatch)
	{
		MatchableCycleID = toMatch.MatchableCycleID;
		MatchableCycle = toMatch.MatchableCycle;
	}

	public override ToMatch ToModel()
	{
		return new(this);
	}
}
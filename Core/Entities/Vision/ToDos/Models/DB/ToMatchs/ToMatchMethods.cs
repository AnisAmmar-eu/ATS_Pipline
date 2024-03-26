using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;

namespace Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

public partial class ToMatch
{
	public ToMatch()
	{
	}

	public ToMatch(DTOToMatch dtoToMatch) : base(dtoToMatch)
	{
		MatchableCycleID = dtoToMatch.MatchableCycleID;
		if (dtoToMatch.MatchableCycle is not null)
			MatchableCycle = dtoToMatch.MatchableCycle;
	}

	public override DTOToMatch ToDTO()
	{
		return new(this);
	}
}
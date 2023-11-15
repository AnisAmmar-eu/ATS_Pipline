using Core.Entities.StationCycles.Models.DB.MatchingCycles;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles;

public partial class DTOMatchingCycle
{
	public DTOMatchingCycle()
	{
	}

	public DTOMatchingCycle(MatchingCycle cycle) : base(cycle)
	{
		MatchingCamera1 = cycle.MatchingCamera1;
		MatchingCamera2 = cycle.MatchingCamera2;
	}
}
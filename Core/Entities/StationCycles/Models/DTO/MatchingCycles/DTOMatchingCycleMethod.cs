using Core.Entities.StationCycles.Models.DB.MatchingCycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DTO.SigningCycles.S1S2Cycles;

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

	public override MatchingCycle ToModel()
	{
		return this switch
		{
			DTOS3S4Cycle dtoS1S2Cycle => dtoS1S2Cycle.ToModel(),
			DTOS5Cycle dtos5Cycle => dtos5Cycle.ToModel(),
			_ => throw new InvalidCastException("Trying to convert an abstract class to model")
		};
	}
}
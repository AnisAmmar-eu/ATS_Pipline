using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;

namespace Core.Entities.StationCycles.Models.DTO.LoadableCycles;

public partial class DTOLoadableCycle
{
	public DTOLoadableCycle()
	{
	}

	public override LoadableCycle ToModel()
	{
		return (this is DTOS1S2Cycle dtoS1S2Cycle)
			? (LoadableCycle)dtoS1S2Cycle.ToModel()
			: throw new InvalidCastException("Trying to convert an abstract class to model");
	}
}
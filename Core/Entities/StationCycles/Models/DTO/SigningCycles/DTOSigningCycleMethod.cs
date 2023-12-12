using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.StationCycles.Models.DTO.SigningCycles.S1S2Cycles;

namespace Core.Entities.StationCycles.Models.DTO.SigningCycles;

public partial class DTOSigningCycle
{
	public DTOSigningCycle()
	{
	}

	public DTOSigningCycle(LoadableCycle cycle) : base(cycle)
	{
	}

	public override LoadableCycle ToModel()
	{
		if (this is DTOS1S2Cycle dtoS1S2Cycle)
			return dtoS1S2Cycle.ToModel();

		throw new InvalidCastException("Trying to convert an abstract class to model");
	}
}
using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.StationCycles.Models.DB.MatchingCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DB.SigningCycles.S1S2Cycles;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX
{
	public AnodeDX()
	{
	}

	public AnodeDX(DTOAnodeDX dtoAnodeDX) : base(dtoAnodeDX)
	{
		S5CycleID = dtoAnodeDX.S5CycleID;
		S5CycleTS = dtoAnodeDX.S5CycleTS;
		S5Cycle = dtoAnodeDX.S5Cycle?.ToModel();
	}

	public AnodeDX(S1S2Cycle cycle) : base(cycle)
	{
	}

	public override DTOAnodeDX ToDTO()
	{
		return new(this);
	}

	public void AddS5Cycle(S5Cycle cycle)
	{
		S5Cycle = cycle;
		S5CycleID = cycle.ID;
		S5CycleTS = cycle.TS;
	}
}
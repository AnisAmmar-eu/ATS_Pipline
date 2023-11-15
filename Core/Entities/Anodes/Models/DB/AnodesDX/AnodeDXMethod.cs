using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.S5Cycles;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX
{
	public AnodeDX()
	{}
	public AnodeDX(S1S2Cycle cycle) : base(cycle)
	{
	}

	public override DTOAnodeDX ToDTO()
	{
		return new DTOAnodeDX(this);
	}

	public void AddS5Cycle(S5Cycle cycle)
	{
		S5Cycle = cycle;
		S5CycleID = cycle.ID;
		S5CycleTS = cycle.TS;
	}
}
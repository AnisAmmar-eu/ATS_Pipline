using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Mapster;

namespace Core.Entities.Anodes.Models.DB.AnodesD20;

public partial class AnodeD20
{
	public AnodeD20()
	{
	}

	public AnodeD20(S1S2Cycle cycle) : base(cycle)
	{
	}

	public override DTOAnodeD20 ToDTO() => this.Adapt<DTOAnodeD20>();
}
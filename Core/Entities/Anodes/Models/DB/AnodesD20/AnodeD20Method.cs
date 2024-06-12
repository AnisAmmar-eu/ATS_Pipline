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
		this.S1S2SignStatus1 = cycle.SignStatus1;
		this.S1S2SignStatus2 = cycle.SignStatus2;
	}

	public override DTOAnodeD20 ToDTO() => this.Adapt<DTOAnodeD20>();
}
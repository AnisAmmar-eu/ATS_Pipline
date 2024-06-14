using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Mapster;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX
{
	public AnodeDX()
	{
	}

	public AnodeDX(S1S2Cycle cycle) : base(cycle)
	{
		this.S1S2SignStatus1 = cycle.SignStatus1;
		this.S1S2SignStatus2 = cycle.SignStatus2;
		this.S1S2TSFirstShooting = cycle.TSFirstShooting;
	}

	public override DTOAnodeDX ToDTO() => this.Adapt<DTOAnodeDX>();
}
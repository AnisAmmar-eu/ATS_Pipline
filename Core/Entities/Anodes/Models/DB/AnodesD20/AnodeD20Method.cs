using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.StationCycles.Models.DB.SigningCycles.S1S2Cycles;

namespace Core.Entities.Anodes.Models.DB.AnodesD20;

public partial class AnodeD20
{
	public AnodeD20()
	{
	}

	public AnodeD20(DTOAnodeD20 dtoAnodeD20) : base(dtoAnodeD20)
	{
	}

	public AnodeD20(S1S2Cycle cycle) : base(cycle)
	{
	}

	public override DTOAnodeD20 ToDTO()
	{
		return new DTOAnodeD20(this);
	}
}
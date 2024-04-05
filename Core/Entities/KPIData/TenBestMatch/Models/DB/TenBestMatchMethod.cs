using Core.Entities.Anodes.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Mapster;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DB;

public partial class TenBestMatch
{
	public TenBestMatch()
	{
	}

	public override DTOTenBestMatch ToDTO()
	{
		return this.Adapt<DTOTenBestMatch>();
	}
}
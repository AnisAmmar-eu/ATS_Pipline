using Core.Entities.Anodes.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Mapster;

namespace Core.Entities.KPIData.KPIs.Models.DB;

public partial class KPI
{
	public KPI()
	{
	}

	public override DTOKPI ToDTO()
	{
		return this.Adapt<DTOKPI>();
	}
}
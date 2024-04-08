using Core.Entities.KPIData.KPIs.Models.DTO;
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
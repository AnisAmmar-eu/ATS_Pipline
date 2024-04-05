using Core.Entities.KPIData.KPIs.Models.DB;
using Mapster;

namespace Core.Entities.KPIData.KPIs.Models.DTO;

public partial class DTOKPI
{
	public DTOKPI()
	{
	}

	public override KPI ToModel()
	{
		return this.Adapt<KPI>();
	}
}
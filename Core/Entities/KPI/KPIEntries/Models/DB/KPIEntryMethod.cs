using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DB;

public partial class KPIEntry : BaseEntity, IBaseEntity<KPIEntry, DTOKPIEntry>
{
	public KPIEntry()
	{
	}

	protected KPIEntry(KPIEntry kpiEntry, KPIC kpiC)
	{
		ID = 0;
		TS = DateTimeOffset.Now;
		StationID = kpiEntry.StationID;
		Value = kpiEntry.Value;
		Period = kpiEntry.Period;
		KPICID = kpiEntry.KPICID;
		KPIC = kpiC;
	}

	public override DTOKPIEntry ToDTO()
	{
		return new DTOKPIEntry(this);
	}
}
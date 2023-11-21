using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;

namespace Core.Entities.KPI.KPIEntries.Models.DB;

public partial class KPIEntry
{
	public KPIEntry()
	{
	}
	
	public KPIEntry(DTOKPIEntry dtoKPIEntry) : base(dtoKPIEntry)
	{
		Value = dtoKPIEntry.Value;
		Period = dtoKPIEntry.Period;
		KPICID = dtoKPIEntry.KPICID;
		KPIC = dtoKPIEntry.KPIC.ToModel();
	}

	protected KPIEntry(KPIEntry kpiEntry, KPIC kpiC)
	{
		ID = 0;
		TS = DateTimeOffset.Now;
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
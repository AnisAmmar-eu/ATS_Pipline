using Core.Entities.KPI.KPIEntries.Models.DB;

namespace Core.Entities.KPI.KPIEntries.Models.DTO;

public partial class DTOKPIEntry
{
	public DTOKPIEntry(KPIEntry kpiEntry)
	{
		Value = kpiEntry.Value;
		Period = kpiEntry.Period;
		KPICID = kpiEntry.KPICID;
		KPIC = kpiEntry.KPIC.ToDTO();
	}

	public override KPIEntry ToModel()
	{
		return new(this);
	}
}
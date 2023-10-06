using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DTO;

public partial class DTOKPIEntry : DTOBaseEntity, IDTO<KPIEntry, DTOKPIEntry>
{
	public DTOKPIEntry(KPIEntry kpiEntry)
	{
		StationID = kpiEntry.StationID;
		Value = kpiEntry.Value;
		Period = kpiEntry.Period;
		KPICID = kpiEntry.KPICID;
		KPIC = kpiEntry.KPIC.ToDTO();
	}
}
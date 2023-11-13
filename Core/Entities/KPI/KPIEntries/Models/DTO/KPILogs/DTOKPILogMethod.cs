using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;

namespace Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;

public partial class DTOKPILog
{
	public DTOKPILog(KPILog kpiLog) : base(kpiLog)
	{
	}
}
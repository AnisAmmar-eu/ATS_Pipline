using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;

public partial class KPILog : KPIEntry, IBaseEntity<KPILog, DTOKPILog>
{
	public KPILog()
	{
	}

	public KPILog(KPIRT kpiRT, KPIC kpiC) : base(kpiRT, kpiC)
	{
	}

	public override DTOKPILog ToDTO()
	{
		return new DTOKPILog(this);
	}
}
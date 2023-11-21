using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;

namespace Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;

public partial class KPIRT
{
	public KPIRT()
	{}
	
	public KPIRT(DTOKPIRT dtoKPIRT) : base(dtoKPIRT)
	{}
	public override DTOKPIRT ToDTO()
	{
		return new DTOKPIRT(this);
	}

	public KPILog ToLog(KPIC kpiC)
	{
		// ID is set to 0 so it may be added into another table.
		return new KPILog(this, kpiC);
	}
}
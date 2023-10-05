using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;

public partial class KPIRT : KPIEntry, IBaseEntity<KPIRT, DTOKPIRT>
{
	public override DTOKPIRT ToDTO()
	{
		return new DTOKPIRT(this);
	}
}
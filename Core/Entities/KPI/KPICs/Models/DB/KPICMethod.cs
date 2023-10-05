using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPICs.Models.DB;

public partial class KPIC : BaseEntity, IBaseEntity<KPIC, DTOKPIC>
{
	public override DTOKPIC ToDTO()
	{
		return new DTOKPIC(this);
	}
}
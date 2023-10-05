using Core.Entities.KPI.KPICs.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPICs.Models.DTO;

public partial class DTOKPIC : DTOBaseEntity, IDTO<KPIC, DTOKPIC>
{
	public DTOKPIC(KPIC kpic)
	{
		RID = kpic.RID;
		Name = kpic.Name;
		Description = kpic.Description;
	}
}
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPICs.Models.DTO;

public partial class DTOKPIC : DTOBaseEntity, IDTO<KPIC, DTOKPIC>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
}
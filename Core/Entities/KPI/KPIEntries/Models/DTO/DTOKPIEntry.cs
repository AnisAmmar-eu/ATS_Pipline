using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DTO;

public partial class DTOKPIEntry : DTOBaseEntity, IDTO<KPIEntry, DTOKPIEntry>
{
	public int KPICID { get; set; }
	public string Value { get; set; }
	public string Period { get; set; }
	public DTOKPIC KPIC { get; set; }
}
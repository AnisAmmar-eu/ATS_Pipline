using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DTO;

public partial class DTOKPIEntry : DTOBaseEntity, IDTO<KPIEntry, DTOKPIEntry>
{
	public int KPICID;
	public int StationID;
	public int Value;
	public string Period;
	public DTOKPIC KPIC;
}
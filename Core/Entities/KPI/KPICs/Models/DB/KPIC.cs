using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPICs.Models.DB;

public partial class KPIC : BaseEntity, IBaseEntity<KPIC, DTOKPIC>
{
	public string RID;
	public string Name;
	public string Description;
	
	#region Nav Properties

	public List<KPILog> LogEntries { get; set; } = new List<KPILog>();
	public List<KPIRT> RTEntries { get; set; } = new List<KPIRT>();

	#endregion
}
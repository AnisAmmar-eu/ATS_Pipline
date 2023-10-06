using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPICs.Models.DB;

public partial class KPIC : BaseEntity, IBaseEntity<KPIC, DTOKPIC>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	#region Nav Properties

	public ICollection<KPILog> LogEntries { get; set; } = new List<KPILog>();
	public ICollection<KPIRT> RTEntries { get; set; } = new List<KPIRT>();

	#endregion
}
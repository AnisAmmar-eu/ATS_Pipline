using System.Text.Json.Serialization;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Models.DTO;

public partial class DTOAlarmC : DTOBaseEntity, IDTO<AlarmC, DTOAlarmC>
{
	// DTO ONLY SENT TO FRONTEND
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public int Severity { get; set; }
	public virtual ICollection<DTOAlarmLog>? Journals { get; set; }
	public virtual DTOAlarmRT? AlarmRT { get; set; }
}
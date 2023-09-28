using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<AlarmC, DTOAlarmC>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	// TODO Refactor nav
	public virtual ICollection<AlarmLog> AlarmLogs { get; set; }

	public virtual AlarmRT AlarmRT { get; set; }
}
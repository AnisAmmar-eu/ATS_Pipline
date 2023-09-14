using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsLog.Models.DB;
using Core.Entities.AlarmsRT.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<AlarmC, DTOAlarmC>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	// TODO Refactor nav
	public virtual ICollection<AlarmLog> AlarmLogs { get; set; }

	public virtual AlarmRT AlarmRT { get; set; }
}
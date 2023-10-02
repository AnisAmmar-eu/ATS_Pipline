using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DB;

public partial class AlarmPLC : BaseEntity, IBaseEntity<AlarmPLC, DTOAlarmPLC>
{
	public int AlarmID { get; set; }
	public bool IsActive { get; set; }
	public bool IsOneShot { get; set; }
}
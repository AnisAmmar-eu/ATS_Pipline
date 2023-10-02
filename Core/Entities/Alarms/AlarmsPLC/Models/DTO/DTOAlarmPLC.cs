using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DTO;

public partial class DTOAlarmPLC : DTOBaseEntity, IDTO<AlarmPLC, DTOAlarmPLC>
{
	public int AlarmID { get; set; }
	public bool IsActive { get; set; }
	public bool IsOneShot { get; set; }
}
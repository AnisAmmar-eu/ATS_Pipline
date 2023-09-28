using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
namespace Core.Entities.Alarms.AlarmsPLC.Models.DTO;

public partial class DTOAlarmPLC : DTOBaseEntity, IDTO<AlarmPLC, DTOAlarmPLC>
{
	public int AlarmID { get; set; }
	public bool IsActive { get; set; }
	public bool IsOneShot { get; set; }
}
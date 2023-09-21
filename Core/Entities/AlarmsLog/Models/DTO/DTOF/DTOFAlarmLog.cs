using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsLog.Models.DB;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmsLog.Models.DTO.DTOF;

public partial class DTOFAlarmLog : DTOAlarmLog
{
	public DTOAlarmC Alarm { get; set; }
}
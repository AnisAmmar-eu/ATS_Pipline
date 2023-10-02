using Core.Entities.Alarms.AlarmsC.Models.DTO;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;

public partial class DTOFAlarmLog : DTOAlarmLog
{
	public DTOAlarmC Alarm { get; set; }
}
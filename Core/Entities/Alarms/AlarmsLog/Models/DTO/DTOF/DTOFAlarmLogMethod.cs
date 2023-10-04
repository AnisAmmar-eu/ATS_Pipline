using Core.Entities.Alarms.AlarmsLog.Models.DB;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;

public partial class DTOFAlarmLog : DTOAlarmLog
{
	public DTOFAlarmLog(AlarmLog alarmLog) : base(alarmLog)
	{
		Alarm = alarmLog.Alarm.ToDTO();
	}
}
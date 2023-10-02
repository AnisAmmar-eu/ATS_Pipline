using Core.Entities.Alarms.AlarmsLog.Models.DB;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;

public partial class DTOFAlarmLog : DTOAlarmLog
{
	public DTOFAlarmLog(AlarmLog alarmLog, string? languageRID = null) : base(alarmLog, languageRID)
	{
		Alarm = alarmLog.Alarm.ToDTO();
	}
}
using Core.Entities.Alarms.AlarmsLog.Models.DB;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;

public partial class DTOSAlarmLog : DTOAlarmLog
{
	public DTOSAlarmLog()
	{
		AlarmRID = "";
	}

	public DTOSAlarmLog(AlarmLog alarmLog, string? languageRID = null) : base(alarmLog, languageRID)
	{
		HasBeenSent = alarmLog.HasBeenSent;
		AlarmRID = alarmLog.Alarm.RID;
		AlarmID = alarmLog.AlarmID;
	}
}
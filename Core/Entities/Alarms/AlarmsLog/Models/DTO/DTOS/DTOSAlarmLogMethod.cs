using Core.Entities.Alarms.AlarmsLog.Models.DB;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;

public partial class DTOSAlarmLog
{
	public DTOSAlarmLog()
	{
		AlarmRID = "";
	}

	public DTOSAlarmLog(AlarmLog alarmLog) : base(alarmLog)
	{
		HasBeenSent = alarmLog.HasBeenSent;
		AlarmRID = alarmLog.Alarm.RID;
		AlarmID = alarmLog.AlarmID;
	}
}
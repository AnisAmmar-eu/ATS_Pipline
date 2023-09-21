using Core.Entities.AlarmsLog.Models.DB;

namespace Core.Entities.AlarmsLog.Models.DTO.DTOS;

public partial class DTOSAlarmLog : DTOAlarmLog
{
	public DTOSAlarmLog() : base()
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
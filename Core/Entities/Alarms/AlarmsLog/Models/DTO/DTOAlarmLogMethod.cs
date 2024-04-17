using System.Text.Json.Serialization;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DB;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO;

public partial class DTOAlarmLog
{
	[JsonConstructor]
	public DTOAlarmLog(DTOAlarmC alarm)
	{
		Alarm = alarm;
		TS = DateTimeOffset.Now;
		IsAck = false;
	}

	public DTOAlarmLog(AlarmLog alarmLog) : base(alarmLog)
	{
		StationID = alarmLog.StationID;
		IsAck = alarmLog.IsAck;
		IsActive = alarmLog.IsActive;
		HasBeenSent = alarmLog.HasBeenSent;
		AlarmID = alarmLog.AlarmID;
		AlarmRID = alarmLog.Alarm.RID;
		Alarm = alarmLog.Alarm.ToDTO();
		TSRaised = alarmLog.TSRaised;
		TSClear = alarmLog.TSClear;
		Duration = alarmLog.Duration;
		TSRead = alarmLog.TSRead;
		TSGet = alarmLog.TSGet;
	}

	public override AlarmLog ToModel() => new(this);
}
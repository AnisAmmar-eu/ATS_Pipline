using Core.Entities.Alarms.AlarmsLog.Models.DB;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO;

public partial class DTOAlarmLog
{
	public DTOAlarmLog()
	{
		TS = DateTime.Now;
		IsAck = false;
	}


	public DTOAlarmLog(AlarmLog alarmLog)
	{
		ID = alarmLog.ID;
		TS = alarmLog.TS;
		StationID = alarmLog.StationID;
		IsAck = alarmLog.IsAck;
		IsActive = alarmLog.IsActive;
		TSRaised = alarmLog.TSRaised;
		TSClear = alarmLog.TSClear;
		Duration = alarmLog.Duration;
		TSRead = alarmLog.TSRead;
		TSGet = alarmLog.TSGet;
	}

	public override AlarmLog ToModel()
	{
		return new AlarmLog(this);
	}
}
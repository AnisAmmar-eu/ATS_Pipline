﻿using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;

namespace Core.Entities.Alarms.AlarmsLog.Models.DB;

public partial class AlarmLog
{
	public AlarmLog()
	{
	}

	public AlarmLog(AlarmC alarmC)
	{
		TS = DateTimeOffset.Now;
		IsAck = false;
		HasBeenSent = false;
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public AlarmLog(DTOAlarmLog dtoAlarmLog) : base(dtoAlarmLog)
	{
		StationID = dtoAlarmLog.StationID;
		IsAck = dtoAlarmLog.IsAck;
		IsActive = dtoAlarmLog.IsActive;
		AlarmID = dtoAlarmLog.AlarmID;
		HasBeenSent = dtoAlarmLog.HasBeenSent;
		TSRaised = dtoAlarmLog.TSRaised;
		TSClear = dtoAlarmLog.TSClear;
		Duration = dtoAlarmLog.Duration;
		TSRead = dtoAlarmLog.TSRead;
		TSGet = dtoAlarmLog.TSGet;
	}

	public override DTOAlarmLog ToDTO() => new(this);
}
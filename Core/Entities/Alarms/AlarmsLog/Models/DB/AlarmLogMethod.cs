using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Models.DB;

public partial class AlarmLog : BaseEntity, IBaseEntity<AlarmLog, DTOAlarmLog>
{
	public AlarmLog()
	{
	}

	public AlarmLog(AlarmC alarmC)
	{
		TS = DateTime.Now;
		IsAck = false;
		HasBeenSent = false;
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public AlarmLog(DTOAlarmLog dtoAlarmLog)
	{
		ID = dtoAlarmLog.ID;
		TS = (DateTimeOffset)dtoAlarmLog.TS!;
		StationID = dtoAlarmLog.StationID;
		IsAck = dtoAlarmLog.IsAck;
		IsActive = dtoAlarmLog.IsActive;
		TSRaised = dtoAlarmLog.TSRaised;
		TSClear = dtoAlarmLog.TSClear;
		Duration = dtoAlarmLog.Duration;
		TSRead = dtoAlarmLog.TSRead;
		TSGet = dtoAlarmLog.TSGet;
	}

	public AlarmLog(DTOFAlarmLog dtofAlarmLog) : this((DTOAlarmLog)dtofAlarmLog)
	{
		Alarm = dtofAlarmLog.Alarm.ToModel();
	}

	public AlarmLog(DTOSAlarmLog dtosAlarmLog, AlarmC alarmC) : this(dtosAlarmLog)
	{
		HasBeenSent = dtosAlarmLog.HasBeenSent;
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public override DTOAlarmLog ToDTO()
	{
		return new DTOAlarmLog(this);
	}

	public DTOFAlarmLog ToDTOF()
	{
		return new DTOFAlarmLog(this);
	}

	public DTOSAlarmLog ToDTOS()
	{
		return new DTOSAlarmLog(this);
	}
}
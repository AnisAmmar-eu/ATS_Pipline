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
		Station = dtoAlarmLog.Station;
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

	public override DTOAlarmLog ToDTO(string? languageRID = null)
	{
		return new DTOAlarmLog(this, languageRID);
	}

	public DTOFAlarmLog ToDTOF(string? languageRID = null)
	{
		return new DTOFAlarmLog(this, languageRID);
	}

	public DTOSAlarmLog ToDTOS(string? languageRID = null)
	{
		return new DTOSAlarmLog(this, languageRID);
	}
}
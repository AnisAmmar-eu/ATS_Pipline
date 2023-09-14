using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsLog.Models.DB;

public partial class AlarmLog : BaseEntity, IBaseEntity<AlarmLog, DTOAlarmLog>
{
	public AlarmLog()
	{
	}

	public AlarmLog(AlarmC alarmC)
	{
		TS = DateTime.Now;
		IsAck = false;
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public AlarmLog(DTOAlarmLog dtoAlarmLog)
	{
		ID = dtoAlarmLog.ID;
		TS = (DateTimeOffset)dtoAlarmLog.TS!;
		HasChanged = dtoAlarmLog.HasChanged;
		IRID = dtoAlarmLog.IRID;
		AlarmID = dtoAlarmLog.AlarmID;
		Station = dtoAlarmLog.Station;
		IsAck = dtoAlarmLog.IsAck;
		IsActive = dtoAlarmLog.IsActive;
		TSRaised = dtoAlarmLog.TSRaised;
		TSClear = dtoAlarmLog.TSClear;
		Duration = dtoAlarmLog.Duration;
		TSRead = dtoAlarmLog.TSRead;
		TSGet = dtoAlarmLog.TSGet;
		Alarm = dtoAlarmLog.Alarm.ToModel();
	}

	public DTOAlarmLog ToDTO(string? languageRID = null)
	{
		return new DTOAlarmLog(this, languageRID);
	}
}
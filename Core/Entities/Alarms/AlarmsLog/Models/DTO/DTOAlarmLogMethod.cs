using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO;

public partial class DTOAlarmLog : DTOBaseEntity, IDTO<AlarmLog, DTOAlarmLog>
{
	public DTOAlarmLog()
	{
		TS = DateTime.Now;
		IsAck = false;
	}


	public DTOAlarmLog(AlarmLog alarmLog, string languageRID)
	{
		ID = alarmLog.ID;
		TS = alarmLog.TS;
		Station = alarmLog.Station;
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
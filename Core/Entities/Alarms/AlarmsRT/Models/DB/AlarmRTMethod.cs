using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
namespace Core.Entities.Alarms.AlarmsRT.Models.DB;

public partial class AlarmRT : BaseEntity, IBaseEntity<Alarms.AlarmsRT.Models.DB.AlarmRT, DTOAlarmRT>
{
	public AlarmRT()
	{
	}

	public AlarmRT(AlarmC alarmC)
	{
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public AlarmRT(DTOAlarmRT dtoAlarmRT)
	{
		ID = dtoAlarmRT.ID;
		TS = (DateTimeOffset)dtoAlarmRT.TS!;
		IRID = dtoAlarmRT.IRID;
		AlarmID = dtoAlarmRT.AlarmID;
		Station = dtoAlarmRT.Station;
		NbNonAck = dtoAlarmRT.NbNonAck;
		IsActive = dtoAlarmRT.IsActive;
		TSRaised = dtoAlarmRT.TSRaised;
		TSClear = dtoAlarmRT.TSClear;
		
		Alarm = dtoAlarmRT.Alarm.ToModel();
	}

	public override DTOAlarmRT ToDTO(string? languageRID = null)
	{
		return new DTOAlarmRT(this, languageRID);
	}
}
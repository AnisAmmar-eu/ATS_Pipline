using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;

namespace Core.Entities.Alarms.AlarmsRT.Models.DB;

public partial class AlarmRT
{
	public AlarmRT()
	{
	}

	public AlarmRT(AlarmC alarmC)
	{
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public AlarmRT(DTOAlarmRT dtoAlarmRT) : base(dtoAlarmRT)
	{
		IRID = dtoAlarmRT.IRID;
		AlarmID = dtoAlarmRT.AlarmID;
		StationID = dtoAlarmRT.StationID;
		NbNonAck = dtoAlarmRT.NbNonAck;
		IsActive = dtoAlarmRT.IsActive;
		TSRaised = dtoAlarmRT.TSRaised;
		TSClear = dtoAlarmRT.TSClear;

		Alarm = dtoAlarmRT.Alarm.ToModel();
	}

	public override DTOAlarmRT ToDTO()
	{
		return new DTOAlarmRT(this);
	}
}
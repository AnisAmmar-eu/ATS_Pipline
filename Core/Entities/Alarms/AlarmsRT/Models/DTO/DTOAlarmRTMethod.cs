using Core.Entities.Alarms.AlarmsRT.Models.DB;

namespace Core.Entities.Alarms.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT
{
	public DTOAlarmRT(AlarmRT alarmRT)
	{
		ID = alarmRT.ID;
		TS = alarmRT.TS;
		IRID = alarmRT.IRID;
		AlarmID = alarmRT.AlarmID;
		StationID = alarmRT.StationID;
		NbNonAck = alarmRT.NbNonAck;
		IsActive = alarmRT.IsActive;
		TSRaised = alarmRT.TSRaised;
		TSClear = alarmRT.TSClear;

		Alarm = alarmRT.Alarm.ToDTO();
	}

	public override AlarmRT ToModel()
	{
		return new AlarmRT(this);
	}
}
using Core.Entities.Alarms.AlarmsRT.Models.DB;

namespace Core.Entities.Alarms.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT
{
	public DTOAlarmRT(AlarmRT alarmRT) : base(alarmRT)
	{
		IRID = alarmRT.IRID;
		AlarmID = alarmRT.AlarmID;
		StationID = alarmRT.StationID;
		IsActive = alarmRT.IsActive;
		TSRaised = alarmRT.TSRaised;

		Alarm = alarmRT.Alarm.ToDTO();
	}

	public override AlarmRT ToModel()
	{
		return new(this);
	}
}
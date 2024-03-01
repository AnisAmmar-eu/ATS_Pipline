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

	public AlarmRT(Alarm alarm)
	{
		IRID = alarm.RID.ToString();
		IsActive = alarm.Value;
		TSRaised = alarm.TS.GetTimestamp();
	}

	public AlarmRT(DTOAlarmRT dtoAlarmRT) : base(dtoAlarmRT)
	{
		IRID = dtoAlarmRT.IRID;
		AlarmID = dtoAlarmRT.AlarmID;
		StationID = dtoAlarmRT.StationID;
		IsActive = dtoAlarmRT.IsActive;
		TSRaised = dtoAlarmRT.TSRaised;

		Alarm = dtoAlarmRT.Alarm.ToModel();
	}

	public override DTOAlarmRT ToDTO()
	{
		return new(this);
	}
}
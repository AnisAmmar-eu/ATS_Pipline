using Core.Entities.Alarms.AlarmsPLC.Models.DB;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DTO;

public partial class DTOAlarmPLC
{
	public DTOAlarmPLC(AlarmPLC alarmPLC)
	{
		ID = alarmPLC.ID;
		TS = alarmPLC.TS;
		AlarmID = alarmPLC.AlarmID;
		IsActive = alarmPLC.IsActive;
		IsOneShot = alarmPLC.IsOneShot;
	}

	public override AlarmPLC ToModel()
	{
		return new AlarmPLC(this);
	}
}
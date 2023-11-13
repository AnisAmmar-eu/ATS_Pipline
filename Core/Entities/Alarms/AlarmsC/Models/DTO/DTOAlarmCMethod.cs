using Core.Entities.Alarms.AlarmsC.Models.DB;

namespace Core.Entities.Alarms.AlarmsC.Models.DTO;

public partial class DTOAlarmC
{
	public DTOAlarmC()
	{
		RID = "";
		Name = "";
		Description = "";
		Category = "";
	}

	public DTOAlarmC(AlarmC alarmC)
	{
		ID = alarmC.ID;
		TS = alarmC.TS;
		RID = alarmC.RID;
		Name = alarmC.Name;
		Description = alarmC.Description;
		Category = alarmC.Category;
		Severity = alarmC.Severity;
	}

	public override AlarmC ToModel()
	{
		return new AlarmC(this);
	}
}
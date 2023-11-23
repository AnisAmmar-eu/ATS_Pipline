using Core.Entities.Alarms.AlarmsC.Models.DB;

namespace Core.Entities.Alarms.AlarmsC.Models.DTO;

public partial class DTOAlarmC
{
	public DTOAlarmC()
	{
	}

	public DTOAlarmC(AlarmC alarmC) : base(alarmC)
	{
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
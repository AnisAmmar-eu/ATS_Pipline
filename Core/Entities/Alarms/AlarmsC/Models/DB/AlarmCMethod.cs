using Core.Entities.Alarms.AlarmsC.Models.DTO;

namespace Core.Entities.Alarms.AlarmsC.Models.DB;

public partial class AlarmC
{
	public AlarmC()
	{
	}

	public AlarmC(DTOAlarmC alarmC) : base(alarmC)
	{
		RID = alarmC.RID;
		Name = alarmC.Name;
		Description = alarmC.Description;
		Category = alarmC.Category;
		Severity = alarmC.Severity;
	}

	public override DTOAlarmC ToDTO()
	{
		return new(this);
	}
}
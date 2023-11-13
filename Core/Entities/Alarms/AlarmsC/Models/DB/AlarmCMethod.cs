using Core.Entities.Alarms.AlarmsC.Models.DTO;

namespace Core.Entities.Alarms.AlarmsC.Models.DB;

public partial class AlarmC
{
	public AlarmC()
	{
		Name = "";
		Description = "";
		RID = "";
		Category = "";
	}

	public AlarmC(DTOAlarmC alarmC)
	{
		ID = alarmC.ID;
		TS = (DateTimeOffset)alarmC.TS!;
		RID = alarmC.RID;
		Name = alarmC.Name;
		Description = alarmC.Description;
		Category = alarmC.Category;
		Severity = alarmC.Severity;
	}

	public override DTOAlarmC ToDTO()
	{
		return new DTOAlarmC(this);
	}
}
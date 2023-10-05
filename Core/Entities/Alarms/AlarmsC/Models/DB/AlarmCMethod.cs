using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<AlarmC, DTOAlarmC>
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
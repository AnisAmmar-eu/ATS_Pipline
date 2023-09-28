using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<Alarms.AlarmsC.Models.DB.AlarmC, DTOAlarmC>
{
	public AlarmC()
	{
		Name = "";
		Description = "";
	}

	public AlarmC(DTOAlarmC alarmC)
	{
		ID = alarmC.ID;
		TS = (DateTimeOffset)alarmC.TS!;
		RID = alarmC.RID;
		Name = alarmC.Name;
		Description = alarmC.Description;
	}

	public override DTOAlarmC ToDTO(string? languageRID = null)
	{
		return new DTOAlarmC(this);
	}
}
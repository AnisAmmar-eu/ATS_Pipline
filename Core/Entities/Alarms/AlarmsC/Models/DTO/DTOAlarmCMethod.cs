using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Models.DTO;

public partial class DTOAlarmC : DTOBaseEntity, IDTO<AlarmC, DTOAlarmC>
{
	public DTOAlarmC()
	{
	}

	public DTOAlarmC(AlarmC alarmC)
	{
		ID = alarmC.ID;
		TS = alarmC.TS;
		RID = alarmC.RID;
		Name = alarmC.Name;
		Description = alarmC.Description;
	}

	public override AlarmC ToModel()
	{
		return new AlarmC(this);
	}
}
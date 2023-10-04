using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DB;

public partial class AlarmPLC : BaseEntity, IBaseEntity<AlarmPLC, DTOAlarmPLC>
{
	public AlarmPLC()
	{
	}

	public AlarmPLC(Alarm alarm)
	{
		TS = DateTimeOffset.FromUnixTimeSeconds(alarm.TimeStamp).AddMilliseconds(alarm.TimeStampMS);
		AlarmID = (int)alarm.ID;
		IsActive = alarm.Status == 1;
		IsOneShot = alarm.OneShot;
	}

	public DTOAlarmPLC ToDTO()
	{
		return new DTOAlarmPLC(this);
	}
}
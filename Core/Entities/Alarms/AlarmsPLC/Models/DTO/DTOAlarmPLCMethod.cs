using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Models.DTO;

public partial class DTOAlarmPLC : DTOBaseEntity, IDTO<AlarmPLC, DTOAlarmPLC>
{
	public DTOAlarmPLC(AlarmPLC alarmPLC)
	{
		ID = alarmPLC.ID;
		TS = alarmPLC.TS;
		AlarmID = alarmPLC.AlarmID;
		IsActive = alarmPLC.IsActive;
		IsOneShot = alarmPLC.IsOneShot;
	}
}
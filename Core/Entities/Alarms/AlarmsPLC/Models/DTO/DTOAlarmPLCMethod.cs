using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
namespace Core.Entities.Alarms.AlarmsPLC.Models.DTO;

public partial class DTOAlarmPLC : DTOBaseEntity, IDTO<AlarmPLC, Alarms.AlarmsPLC.Models.DTO.DTOAlarmPLC>
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
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmsPLC.Models.DTOs;

public partial class DTOAlarmPLC : DTOBaseEntity, IDTO<AlarmPLC, DTOAlarmPLC>
{
	public DTOAlarmPLC(AlarmPLC alarmPLC)
	{
		ID = alarmPLC.ID;
		TS = alarmPLC.TS;
		IDAlarm = alarmPLC.IDAlarm;
		Status = alarmPLC.Status;
	}
}
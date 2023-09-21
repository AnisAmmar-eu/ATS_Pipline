using Core.Entities.AlarmsCycle.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmsCycle.Models.DTO;

public partial class DTOAlarmCycle : DTOBaseEntity, IDTO<AlarmCycle, DTOAlarmCycle>
{
	public DTOAlarmCycle(AlarmCycle alarmCycle)
	{
		AlarmRID = alarmCycle.AlarmRID;
		NbNonAck = alarmCycle.NbNonAck;
		IsActive = alarmCycle.IsActive;
		AlarmListPacketID = alarmCycle.AlarmListPacketID;
	}

	public override AlarmCycle ToModel()
	{
		return new AlarmCycle(this);
	}
}
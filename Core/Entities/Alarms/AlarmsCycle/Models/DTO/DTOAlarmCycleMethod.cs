using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DTO;

public partial class DTOAlarmCycle : DTOBaseEntity, IDTO<AlarmCycle, DTOAlarmCycle>
{
	public DTOAlarmCycle()
	{
		AlarmRID = "";
	}

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
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DB;

public partial class AlarmCycle : BaseEntity, IBaseEntity<AlarmCycle, DTOAlarmCycle>
{
	public AlarmCycle()
	{
		AlarmRID = "";
	}

	public AlarmCycle(AlarmRT alarmRT)
	{
		AlarmRID = alarmRT.Alarm.RID;
		NbNonAck = alarmRT.NbNonAck;
		IsActive = alarmRT.IsActive;
	}

	public AlarmCycle(DTOAlarmCycle dtoAlarmCycle)
	{
		AlarmRID = dtoAlarmCycle.AlarmRID;
		NbNonAck = dtoAlarmCycle.NbNonAck;
		IsActive = dtoAlarmCycle.IsActive;
		AlarmListPacketID = dtoAlarmCycle.AlarmListPacketID;
	}

	public override DTOAlarmCycle ToDTO()
	{
		return new DTOAlarmCycle(this);
	}
}
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Models.DB;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DB;

public partial class AlarmCycle
{
	public AlarmCycle()
	{
		AlarmRID = string.Empty;
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
		return new(this);
	}
}
using Core.Entities.Alarms.AlarmsCycle.Models.DB;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DTO;

public partial class DTOAlarmCycle
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
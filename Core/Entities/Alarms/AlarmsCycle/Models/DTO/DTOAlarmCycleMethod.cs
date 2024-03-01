using Core.Entities.Alarms.AlarmsCycle.Models.DB;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DTO;

public partial class DTOAlarmCycle
{
	public DTOAlarmCycle()
	{
		AlarmRID = string.Empty;
	}

	public DTOAlarmCycle(AlarmCycle alarmCycle)
	{
		AlarmRID = alarmCycle.AlarmRID;
		IsActive = alarmCycle.IsActive;
		AlarmListPacketID = alarmCycle.AlarmListPacketID;
	}

	public override AlarmCycle ToModel()
	{
		return new(this);
	}
}
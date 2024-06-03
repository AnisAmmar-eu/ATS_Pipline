using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Shared.UnitOfWork.Interfaces;
using Mapster;

namespace Core.Entities.Packets.Models.DB.AlarmLists;

public partial class AlarmList
{
	public AlarmList()
	{
		AlarmCycles = [];
	}

	public override DTOAlarmList ToDTO() => this.Adapt<DTOAlarmList>();

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		List<AlarmRT> alarmRTs = await anodeUOW.AlarmRT.GetAllWithInclude(
			[alarm => alarm.IsActive], withTracking: false);
		foreach (AlarmRT alarmRT in alarmRTs)
		{
			AlarmCycle alarmCycle = new(alarmRT) {
				AlarmListPacketID = ID,
				AlarmList = this,
			};
			await anodeUOW.AlarmCycle.Add(alarmCycle);
			AlarmCycles.Add(alarmCycle);
		}

		if (AlarmCycles.Count != 0)
			anodeUOW.Commit();

		Status = PacketStatus.Completed;
	}
}
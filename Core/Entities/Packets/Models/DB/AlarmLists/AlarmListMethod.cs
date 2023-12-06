using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.AlarmLists;

public partial class AlarmList
{
	public AlarmList()
	{
		AlarmCycles = new List<AlarmCycle>();
	}

	public AlarmList(DTOAlarmList dto) : base(dto)
	{
		AlarmCycles = dto.AlarmCycles.ToList().ConvertAll(dtoAlarmCycle =>
		{
			AlarmCycle alarmCycle = dtoAlarmCycle.ToModel();
			alarmCycle.AlarmList = this;
			return alarmCycle;
		});
	}

	public override DTOAlarmList ToDTO()
	{
		return new(this);
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		List<AlarmRT> alarmRTs = await anodeUOW.AlarmRT.GetAllWithInclude(withTracking: false);
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

		// StationCycleRID should already be present
		StationCycle ??= await anodeUOW.StationCycle
			.GetBy([stationCycle => stationCycle.RID == StationCycleRID], withTracking: false);

		StationCycle.AlarmListPacket = this;
		StationCycle.AlarmListID = ID;
		Status = PacketStatus.Completed;
		StationCycle.AlarmListStatus = Status;
		anodeUOW.StationCycle.Update(StationCycle);
	}
}
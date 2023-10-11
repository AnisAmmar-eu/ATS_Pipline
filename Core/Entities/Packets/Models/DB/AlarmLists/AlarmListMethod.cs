using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.AlarmLists;

public partial class AlarmList : Packet, IBaseEntity<AlarmList, DTOAlarmList>
{
	public AlarmList()
	{
		Type = PacketType.Alarm;
		AlarmCycles = new List<AlarmCycle>();
	}

	public AlarmList(DTOAlarmList dto) : base(dto)
	{
		Type = PacketType.Alarm;
		AlarmCycles = dto.AlarmList.ToList().ConvertAll(dtoAlarmCycle =>
		{
			AlarmCycle alarmCycle = dtoAlarmCycle.ToModel();
			alarmCycle.AlarmList = this;
			return alarmCycle;
		});
	}

	public override DTOAlarmList ToDTO()
	{
		return new DTOAlarmList(this);
	}

	protected override async Task<DTOPacket> InheritedBuild(IAnodeUOW anodeUOW, DTOPacket dtoPacket)
	{
		DTOAlarmList dtoAlarmList = (DTOAlarmList)dtoPacket;
		List<AlarmRT> alarmRTs = await anodeUOW.AlarmRT.GetAllWithInclude(withTracking: false);
		foreach (AlarmRT alarmRT in alarmRTs)
		{
			AlarmCycle alarmCycle = new(alarmRT);
			alarmCycle.AlarmListPacketID = ID;
			alarmCycle.AlarmList = this;
			await anodeUOW.AlarmCycle.Add(alarmCycle);
			anodeUOW.Commit();
			dtoAlarmList.AlarmList.Add(alarmCycle.ToDTO());
		}
		// StationCycleRID should already be present
		if (StationCycle == null)
			StationCycle = await anodeUOW.StationCycle.GetBy(filters: new Expression<Func<StationCycle, bool>>[]
			{
				stationCycle => stationCycle.RID == StationCycleRID
			}, withTracking: false);
		
		StationCycle.AlarmListPacket = this;
		StationCycle.AlarmListID = ID;
		Status = PacketStatus.Completed;
		StationCycle.AlarmListStatus = Status;
		anodeUOW.StationCycle.Update(StationCycle);

		return dtoAlarmList;
	}
}
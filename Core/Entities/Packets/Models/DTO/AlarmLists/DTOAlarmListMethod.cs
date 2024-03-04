using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.AlarmLists;

namespace Core.Entities.Packets.Models.DTO.AlarmLists;

public partial class DTOAlarmList
{
	public DTOAlarmList()
	{
		Type = PacketTypes.AlarmList;
		AlarmCycles = new List<DTOAlarmCycle>();
	}

	public DTOAlarmList(AlarmList packet) : base(packet)
	{
		Type = PacketTypes.AlarmList;
		AlarmCycles = packet.AlarmCycles.ToList().ConvertAll(alarmCycle => alarmCycle.ToDTO());
	}

	public override AlarmList ToModel()
	{
		return new(this);
	}
}
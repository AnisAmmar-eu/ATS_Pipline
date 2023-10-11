using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.AlarmLists;

public partial class DTOAlarmList : DTOPacket, IDTO<AlarmList, DTOAlarmList>
{
	public DTOAlarmList()
	{
		Type = PacketType.Alarm;
		AlarmList = new List<DTOAlarmCycle>();
	}

	public DTOAlarmList(AlarmList packet) : base(packet)
	{
		Type = PacketType.Alarm;
		AlarmList = packet.AlarmCycles.ToList().ConvertAll(alarmCycle => alarmCycle.ToDTO());
	}

	public override AlarmList ToModel()
	{
		return new AlarmList(this);
	}
}
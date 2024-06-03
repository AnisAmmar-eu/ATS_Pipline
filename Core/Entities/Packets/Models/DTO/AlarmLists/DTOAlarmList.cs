using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.AlarmLists;

public partial class DTOAlarmList : DTOPacket, IDTO<AlarmList, DTOAlarmList>
{
	new public string Type { get; set; } = PacketTypes.AlarmList;
	public ICollection<DTOAlarmCycle> AlarmCycles { get; set; } = [];
}
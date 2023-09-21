using Core.Entities.AlarmsCycle.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using DTOAlarmListPacket = Core.Entities.Packets.Models.DTO.AlarmListPackets.DTOAlarmListPacket;

namespace Core.Entities.Packets.Models.DB.AlarmListPackets;

public partial class AlarmListPacket : Packet, IBaseEntity<AlarmListPacket, DTOAlarmListPacket>
{
	public ICollection<AlarmCycle> AlarmList { get; set; } = new List<AlarmCycle>();
}
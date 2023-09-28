using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Packets.Models.DTO.AlarmListPackets;
namespace Core.Entities.Packets.Models.DB.AlarmListPackets;

public partial class AlarmListPacket : Packet, IBaseEntity<AlarmListPacket, DTOAlarmListPacket>
{
	public ICollection<AlarmCycle> AlarmList { get; set; } = new List<AlarmCycle>();
}
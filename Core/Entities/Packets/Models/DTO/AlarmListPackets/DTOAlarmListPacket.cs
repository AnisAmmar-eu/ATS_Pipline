using Core.Entities.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Models.DB.AlarmListPackets;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.AlarmListPackets;

public partial class DTOAlarmListPacket : DTOPacket, IDTO<AlarmListPacket, AlarmListPackets.DTOAlarmListPacket>
{
	public ICollection<DTOAlarmCycle> AlarmList { get; set; } = new List<DTOAlarmCycle>();
}
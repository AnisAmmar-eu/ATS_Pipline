using Core.Entities.Packets.Models.DB.AlarmListPackets;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
namespace Core.Entities.Packets.Models.DTO.AlarmListPackets;

public partial class DTOAlarmListPacket : DTOPacket, IDTO<AlarmListPacket, AlarmListPackets.DTOAlarmListPacket>
{
	public ICollection<DTOAlarmCycle> AlarmList { get; set; } = new List<DTOAlarmCycle>();
}
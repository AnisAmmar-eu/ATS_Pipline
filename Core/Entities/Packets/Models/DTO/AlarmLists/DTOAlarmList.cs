using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Shared.Models.DTOs.Kernel.Interfaces;


namespace Core.Entities.Packets.Models.DTO.AlarmLists;

public partial class DTOAlarmList : DTOPacket, IDTO<AlarmList, DTOAlarmList>
{
	public ICollection<DTOAlarmCycle> AlarmList { get; set; } = new List<DTOAlarmCycle>();
}
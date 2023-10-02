using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.AlarmLists;

public partial class AlarmList : Packet, IBaseEntity<AlarmList, DTOAlarmList>
{
	public ICollection<AlarmCycle> AlarmCycles { get; set; } = new List<AlarmCycle>();
}
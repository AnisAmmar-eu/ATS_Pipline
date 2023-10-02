using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DTO;

public partial class DTOAlarmCycle : DTOBaseEntity, IDTO<AlarmCycle, DTOAlarmCycle>
{
	// DTO ONLY SENT TO SERVER

	public string AlarmRID { get; set; }
	public int? NbNonAck { get; set; }
	public bool IsActive { get; set; }

	public int AlarmListPacketID { get; set; }
}
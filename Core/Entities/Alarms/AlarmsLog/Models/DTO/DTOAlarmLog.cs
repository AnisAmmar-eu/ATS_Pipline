using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO;

public partial class DTOAlarmLog : DTOBaseEntity, IDTO<AlarmLog, DTOAlarmLog>
{
	public string? Station { get; set; }
	public bool IsAck { get; set; } // Ack = Acknowledge
	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DateTimeOffset? TSClear { get; set; }
	public TimeSpan? Duration { get; set; }
	public DateTimeOffset? TSRead { get; set; }
	public DateTimeOffset? TSGet { get; set; }
}
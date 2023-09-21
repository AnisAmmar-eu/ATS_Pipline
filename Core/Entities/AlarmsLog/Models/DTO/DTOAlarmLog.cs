using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Models.DTO;

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
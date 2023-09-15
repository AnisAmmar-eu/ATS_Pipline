using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsLog.Models.DB;

public partial class AlarmLog : BaseEntity, IBaseEntity<AlarmLog, DTOAlarmLog>
{
	public bool HasBeenSent { get; set; }
	public string? IRID { get; set; } // Useful?
	public int AlarmID { get; set; }
	public string? Station { get; set; }
	public bool IsAck { get; set; } // Ack = Acknowledge
	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DateTimeOffset? TSClear { get; set; }
	public TimeSpan? Duration { get; set; }
	public DateTimeOffset? TSRead { get; set; }
	public DateTimeOffset? TSGet { get; set; } // Useful? TSGet == TS ?


	private AlarmC? _alarm;

	public AlarmC Alarm
	{
		set => _alarm = value;
		get => _alarm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Alarm));
	}
}
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Models.DB;

public partial class AlarmRT : BaseEntity, IBaseEntity<AlarmRT, DTOAlarmRT>
{
	private AlarmC? _alarm;
	public string IRID { get; set; }
	public int AlarmID { get; set; }
	public string? Station { get; set; }
	public int? NbNonAck { get; set; }
	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DateTimeOffset? TSClear { get; set; }

	public AlarmC Alarm
	{
		set => _alarm = value;
		get => _alarm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Alarm));
	}
}
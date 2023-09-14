using Core.Entities.AlarmsC.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using DTOAlarmRT = Core.Entities.AlarmsRT.Models.DTO.DTOAlarmRT;

namespace Core.Entities.AlarmsRT.Models.DB;

public partial class AlarmRT : BaseEntity, IBaseEntity<AlarmRT, DTOAlarmRT>
{
	public int AlarmID { get; set; }

	public bool IsActive { get; set; }

	public string? Station { get; set; }
	public int? NumberNonRead { get; set; }

	private AlarmC? _alarm;

	public AlarmC Alarm
	{
		set => _alarm = value;
		get => _alarm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Alarm));
	}
}
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Models.DB;

public partial class AlarmRT : BaseEntity, IBaseEntity<AlarmRT, DTOAlarmRT>
{
	public string IRID { get; set; } = string.Empty;
	public int AlarmID { get; set; }
	public int StationID { get; set; } = Station.ID;
	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }

	#region Nav Properties

	private AlarmC? _alarm;

	public AlarmC Alarm
	{
		set => _alarm = value;
		get => _alarm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Alarm));
	}

	#endregion
}
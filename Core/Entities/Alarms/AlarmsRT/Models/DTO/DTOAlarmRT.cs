using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT : DTOBaseEntity, IDTO<AlarmRT, DTOAlarmRT>
{
	public string IRID { get; set; }
	public int AlarmID { get; set; }
	public string? Station { get; set; }
	public int? NbNonAck { get; set; }
	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DateTimeOffset? TSClear { get; set; }
	public DTOAlarmC Alarm { get; set; }
}
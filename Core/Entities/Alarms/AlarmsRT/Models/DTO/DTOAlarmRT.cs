using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT : DTOBaseEntity, IDTO<AlarmRT, DTOAlarmRT>
{
	public string IRID { get; set; } = string.Empty;
	public int AlarmID { get; set; }
	public int StationID { get; set; } = Station.ID;
	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DTOAlarmC Alarm { get; set; }
}
namespace Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;

public partial class DTOSAlarmLog : DTOAlarmLog
{
	public bool HasBeenSent { get; set; }
	public string AlarmRID { get; set; }
	public int AlarmID { get; set; }
}
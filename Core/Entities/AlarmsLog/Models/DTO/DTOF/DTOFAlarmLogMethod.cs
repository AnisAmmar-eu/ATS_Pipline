using Core.Migrations;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Entities.AlarmsLog.Models.DB;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Models.DTO.DTOF;

public partial class DTOFAlarmLog : DTOAlarmLog
{
	public DTOFAlarmLog(AlarmLog alarmLog, string? languageRID = null) : base(alarmLog, languageRID)
	{
		Alarm = alarmLog.Alarm.ToDTO();
	}	
}
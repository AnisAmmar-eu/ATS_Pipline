using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Services;

public interface IAlarmLogService : IServiceBaseEntity<AlarmLog, DTOAlarmLog>
{
	Task Collect(Alarm alarm);
	Task<List<DTOAlarmLog>> GetAllForFront();
	public Task<List<DTOAlarmLog>> GetByClassID(int alarmID);
	Task<List<DTOAlarmLog>> AckAlarmLogs(int[] idAlarmLogs);
	public Task<HttpResponseMessage> SendLogsToServer();
}
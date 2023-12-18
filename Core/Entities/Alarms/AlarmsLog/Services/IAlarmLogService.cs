using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Services;

public interface IAlarmLogService : IBaseEntityService<AlarmLog, DTOAlarmLog>
{
	Task Collect(Alarm alarm);
	Task<int> AckAlarmLogs(int[] idAlarmLogs);
	public Task<HttpResponseMessage> SendLogsToServer();
}
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Services;

public interface IAlarmLogService : IServiceBaseEntity<AlarmLog, DTOAlarmLog>
{
	Task<IEnumerable<DTOAlarmPLC>> Collect();
	Task<List<DTOFAlarmLog>> GetAllForFront();
	public Task<List<DTOFAlarmLog>> GetByClassID(int alarmID);
	Task<List<DTOFAlarmLog>> AckAlarmLogs(int[] idAlarmLogs);
	public Task<HttpResponseMessage> SendLogsToServer();
}
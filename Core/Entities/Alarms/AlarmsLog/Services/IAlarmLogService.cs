using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
namespace Core.Entities.Alarms.AlarmsLog.Services;

public interface IAlarmLogService
{
	Task<DTOAlarmLog> AddAlarmLog(AlarmLog alarmLog);
	// Task<IEnumerable<DTOAlarmPLC>> AddAlarmLogFromPush(AlarmLog alarmLog);
	Task<IEnumerable<DTOAlarmPLC>> Collect();
	Task<int> CollectCyc(int nbSeconds);
	Task<List<DTOFAlarmLog>> GetAll();
	public Task<List<DTOFAlarmLog>> GetByClassID(int alarmID);
	Task<List<DTOFAlarmLog>> AckAlarmLogs(int[] idAlarmLogs);
	public Task<HttpResponseMessage> SendLogsToServer();
}
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.AlarmsPLC.Models.DTOs;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Services;

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
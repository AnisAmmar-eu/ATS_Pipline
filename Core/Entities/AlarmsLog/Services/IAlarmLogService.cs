using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsPLC.Models.DTOs;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Services;

public interface IAlarmLogService
{
	Task<DTOAlarmLog> AddJournal(AlarmLog alarmLog);

	// Task<IEnumerable<DTOAlarmPLC>> AddJournalFromPush(AlarmLog alarmLog);

	Task<IEnumerable<DTOAlarmPLC>> Collect();

	Task<int> CollectCyc(int nbSeconds);

	Task<List<DTOAlarmLog>> GetAllAlarmLog();


	Task<DTOAlarmLog> ReadAlarmLog(int idJournal);
}
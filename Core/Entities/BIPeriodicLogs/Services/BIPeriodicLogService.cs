using Core.Entities.BIPeriodicLogs.Models.DB;
using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Entities.BIPeriodicLogs.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Services;

public class BIPeriodicLogService : ServiceBaseEntity<IBIPeriodicLogRepository, BIPeriodicLog, DTOBIPeriodicLog>,
	IBIPeriodicLogService
{
	protected BIPeriodicLogService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}

	public async Task<DTOBIPeriodicLog> BuildBIPeriodicLog(DTOBIPeriodicLog dtoLog)
	{
		await AlarmUOW.StartTransaction();
		
		dtoLog.ID = 0;
		BIPeriodicLog log = dtoLog.ToModel();
		await log.Create(AlarmUOW);

		await log.Build(AlarmUOW, log.ToDTO());

		await AlarmUOW.CommitTransaction();
		return log.ToDTO();
	}
}
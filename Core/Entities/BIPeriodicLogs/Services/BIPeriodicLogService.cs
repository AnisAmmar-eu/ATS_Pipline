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
}
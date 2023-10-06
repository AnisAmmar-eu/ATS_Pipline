using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Repositories.KPILogs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPILogs;

public class KPILogService : ServiceBaseEntity<IKPILogRepository, KPILog, DTOKPILog>, IKPILogService
{
	public KPILogService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}
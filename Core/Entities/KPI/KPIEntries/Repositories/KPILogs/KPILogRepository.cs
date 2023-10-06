using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Repositories.KPILogs;

public class KPILogRepository : RepositoryBaseEntity<AlarmCTX, KPILog, DTOKPILog>, IKPILogRepository
{
	public KPILogRepository(AlarmCTX context) : base(context)
	{
	}
}
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPI.KPIEntries.Repositories.KPILogs;

public class KPILogRepository : BaseEntityRepository<AnodeCTX, KPILog, DTOKPILog>, IKPILogRepository
{
	public KPILogRepository(AnodeCTX context) : base(context)
	{
	}
}
using Core.Entities.BIPeriodicLogs.Models.DB;
using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.BIPeriodicLogs.Repositories;

public class BIPeriodicLogRepository : RepositoryBaseEntity<AlarmCTX, BIPeriodicLog, DTOBIPeriodicLog>,
	IBIPeriodicLogRepository
{
	public BIPeriodicLogRepository(AlarmCTX context) : base(context)
	{
	}
}
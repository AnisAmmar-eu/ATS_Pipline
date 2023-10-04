using Core.Entities.BIPeriodicLogs.Models.DB;
using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Repositories;

public interface IBIPeriodicLogRepository : IRepositoryBaseEntity<BIPeriodicLog, DTOBIPeriodicLog>
{
}
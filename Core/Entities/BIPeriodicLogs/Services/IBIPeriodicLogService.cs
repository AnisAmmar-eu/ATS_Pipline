using Core.Entities.BIPeriodicLogs.Models.DB;
using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Services;

public interface IBIPeriodicLogService : IServiceBaseEntity<BIPeriodicLog, DTOBIPeriodicLog>
{
}
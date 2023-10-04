using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Shared.Repositories.System.Logs;

public interface ILogRepository : IRepositoryBaseEntity<Log, DTOLog>
{
}
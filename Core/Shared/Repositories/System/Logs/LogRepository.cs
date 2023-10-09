using Core.Shared.Data;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.Kernel;

namespace Core.Shared.Repositories.System.Logs;

public class LogRepository : RepositoryBaseEntity<AnodeCTX, Log, DTOLog>, ILogRepository
{
	public LogRepository(AnodeCTX context) : base(context)
	{
	}
}
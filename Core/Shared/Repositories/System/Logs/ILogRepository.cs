using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Shared.Repositories.System.Logs;

public interface ILogRepository : IBaseEntityRepository<LogEntry, DTOLogEntry>
{
	public Task<List<LogEntry>> GetRange(int start, int nbItems);
	public Task DeleteAll();
}
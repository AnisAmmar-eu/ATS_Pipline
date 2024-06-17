using Core.Shared.Data;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Repositories.System.Logs;

public class LogRepository : BaseEntityRepository<AnodeCTX, LogEntry, DTOLogEntry>, ILogRepository
{
	public LogRepository(AnodeCTX context) : base(context, [], [])
	{
	}

	public Task<List<LogEntry>> GetRange(int start, int nbItems)
		=> _context.Logs.OrderByDescending(log => log.TS).Skip(start).Take(nbItems).ToListAsync();

	public async Task DeleteAll()
	{
		_context.Log.RemoveRange(_context.Log);
		await _context.SaveChangesAsync();
	}
}
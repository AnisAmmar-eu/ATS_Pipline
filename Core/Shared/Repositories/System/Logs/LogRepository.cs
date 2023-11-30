using Core.Shared.Data;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Repositories.System.Logs;

public class LogRepository : BaseEntityRepository<AnodeCTX, Log, DTOLog>, ILogRepository
{
	public LogRepository(AnodeCTX context) : base(context)
	{
	}

	public Task<List<Log>> GetRange(int start, int nbItems)
	{
		return Context.Log.OrderByDescending(log => log.TS).Skip(start).Take(nbItems).ToListAsync();
	}

	public async Task DeleteAll()
	{
		Context.Log.RemoveRange(Context.Log);
		await Context.SaveChangesAsync();
	}
}
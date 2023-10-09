using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.System.Logs;

public class LogsService : ILogsService
{
	private readonly IAnodeUOW _anodeUOW;

	public LogsService(IAnodeUOW anodeUOW)
	{
		_anodeUOW = anodeUOW;
	}

	public async Task<List<DTOLog>> GetAll()
	{
		return (await _anodeUOW.Log.GetAll(
				withTracking: false,
				orderBy: query => query.OrderByDescending(l => l.TS))
			)
			.Select(l => l.ToDTO())
			.ToList();
	}

	public async Task Create(
		DateTimeOffset date,
		string server,
		string api,
		string controller,
		string function,
		string endpoint,
		int code,
		string value
	)
	{
		await _anodeUOW.Log.Add(new Log(server, api, controller, function, endpoint, code, value));
		_anodeUOW.Commit();
	}
}
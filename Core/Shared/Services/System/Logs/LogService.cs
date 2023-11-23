using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.System.Logs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.System.Logs;

public class LogService : ServiceBaseEntity<ILogRepository, Log, DTOLog>, ILogService
{
	public LogService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOLog>> GetAll()
	{
		return (await AnodeUOW.Log.GetAll(
				withTracking: false,
				orderBy: query => query.OrderByDescending(l => l.TS))
			)
			.Select(l => l.ToDTO())
			.ToList();
	}

	public async Task<List<DTOLog>> GetRange(int start, int nbItems)
	{
		return (await AnodeUOW.Log.GetRange(start, nbItems)).ConvertAll(log => log.ToDTO());
	}

	public async Task<List<Log>> GetAllUnsent()
	{
		return await AnodeUOW.Log.GetAll(new Expression<Func<Log, bool>>[]
		{
			log => !log.HasBeenSent
		}, withTracking: false);
	}

	public async Task SendLogs(List<Log> logs, string address)
	{
		if (!logs.Any())
			return;
		using HttpClient httpClient = new();
		StringContent content =
			new(JsonSerializer.Serialize(logs.ConvertAll(cycle => cycle.ToDTO())), Encoding.UTF8,
				"application/json");
		HttpResponseMessage response = await httpClient.PostAsync($"{address}/apiServerReceive/logs", content);
		if (response.IsSuccessStatusCode)
		{
			if (!logs.Any()) return;
			await AnodeUOW.StartTransaction();
			logs.ForEach(log =>
			{
				log.HasBeenSent = true;
				AnodeUOW.Log.Update(log);
			});
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
	}

	public async Task ReceiveLogs(List<DTOLog> dtoLogs)
	{
		// DbContext operations should NOT be done concurrently. Hence why await in loop.
		await AnodeUOW.StartTransaction();
		foreach (DTOLog dto in dtoLogs)
		{
			Log log = dto.ToModel();
			log.ID = 0;
			log.HasBeenSent = true;
			await AnodeUOW.Log.Add(log);
		}

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	public async Task DeleteAllLogs()
	{
		await AnodeUOW.Log.DeleteAll();
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
		await AnodeUOW.Log.Add(new Log(server, api, controller, function, endpoint, code, value, Station.ID));
		AnodeUOW.Commit();
	}
}
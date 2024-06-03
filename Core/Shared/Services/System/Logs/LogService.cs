using System.Text;
using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.System.Logs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.SystemApp.Logs;

public class LogService : BaseEntityService<ILogRepository, Log, DTOLog>, ILogService
{
	public LogService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOLog>> GetAll()
	{
		return (await _anodeUOW.Log.GetAll(
			orderBy: query => query.OrderByDescending(l => l.TS),
			withTracking: false))
			.ConvertAll(l => l.ToDTO());
	}

	public async Task<List<DTOLog>> GetRange(int start, int nbItems)
		=> (await _anodeUOW.Log.GetRange(start, nbItems)).ConvertAll(log => log.ToDTO());

	public Task<List<Log>> GetAllUnsent() => _anodeUOW.Log.GetAll([log => !log.HasBeenSent], withTracking: false);

	public async Task SendLogs(List<Log> logs)
	{
		if (logs.Count == 0)
			return;

		using HttpClient httpClient = new();
		StringContent content
			= new(
				JsonSerializer.Serialize(logs.ConvertAll(cycle => cycle.ToDTO())),
				Encoding.UTF8,
				"application/json");
		HttpResponseMessage response = await httpClient.PostAsync(
			$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/logs",
			content);
		if (!response.IsSuccessStatusCode)
			return;

		if (logs.Count == 0)
			return;

		await _anodeUOW.StartTransaction();
		logs.ForEach(log => {
			log.HasBeenSent = true;
			_anodeUOW.Log.Update(log);
		});
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	public async Task ReceiveLogs(List<DTOLog> dtoLogs)
	{
		// DbContext operations should NOT be done concurrently. Hence why await in loop.
		await _anodeUOW.StartTransaction();
		foreach (DTOLog dto in dtoLogs)
		{
			Log log = dto.ToModel();
			log.ID = 0;
			log.HasBeenSent = true;
			await _anodeUOW.Log.Add(log);
		}

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	public Task DeleteAllLogs() => _anodeUOW.Log.DeleteAll();

	public async Task Create(
		DateTimeOffset date,
		string server,
		string username,
		string api,
		string controller,
		string function,
		string endpoint,
		int code,
		string value
		)
	{
		await _anodeUOW.Log.Add(new Log(server, username, api, controller, function, endpoint, code, value, Station.ID));
		_anodeUOW.Commit();
	}
}
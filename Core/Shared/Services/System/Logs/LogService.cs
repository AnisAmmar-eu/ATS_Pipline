using System.Text;
using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Repositories.System.Logs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.SystemApp.Logs;

public class LogService : BaseEntityService<ILogRepository, LogEntry, DTOLogEntry>, ILogService
{
	public LogService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOLogEntry>> GetAll()
	{
		return (await _anodeUOW.Logs.GetAll(
			orderBy: query => query.OrderByDescending(l => l.TS),
			withTracking: false))
			.ConvertAll(l => l.ToDTO());
	}

	public async Task<List<DTOLogEntry>> GetRange(int start, int nbItems)
		=> (await _anodeUOW.Logs.GetRange(start, nbItems)).ConvertAll(log => log.ToDTO());

	public Task<List<LogEntry>> GetAllUnsent() => _anodeUOW.Logs.GetAll([log => !log.HasBeenSent], withTracking: false);

	public async Task SendLogs(List<LogEntry> logs)
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
			_anodeUOW.Logs.Update(log);
		});
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	public async Task ReceiveLogs(List<DTOLogEntry> dtoLogs)
	{
		// DbContext operations should NOT be done concurrently. Hence why await in loop.
		await _anodeUOW.StartTransaction();
		foreach (DTOLogEntry dto in dtoLogs)
		{
			LogEntry log = dto.ToModel();
			log.ID = 0;
			log.HasBeenSent = true;
			await _anodeUOW.Logs.Add(log);
		}

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	public Task DeleteAllLogs() => _anodeUOW.Logs.DeleteAll();
}
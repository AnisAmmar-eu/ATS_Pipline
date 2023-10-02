using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Shared.Services.System.Logs
{
	public class LogsService : ILogsService
	{
		private readonly IAlarmUOW _alarmUOW;
		public LogsService(IAlarmUOW alarmUOW)
		{
			_alarmUOW = alarmUOW;
		}

		public async Task<List<DTOLog>> GetAll()
		{
			return (await _alarmUOW.Log.GetAll(
				withTracking: false,
				orderBy: (query) => query.OrderByDescending(l => l.TS))
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
			await _alarmUOW.Log.Add(new(server, api, controller, function, endpoint, code, value));
			_alarmUOW.Commit();
		}
	}
}

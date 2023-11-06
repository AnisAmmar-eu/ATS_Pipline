using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Services.System.Logs;

public interface ILogService
{
	Task<List<DTOLog>> GetAll();
	public Task<List<Log>> GetAllUnsent();
	public Task SendLogs(List<Log> logs, string address);
	public Task ReceiveLogs(List<DTOLog> dtoLogs);

	Task Create(
		DateTimeOffset date,
		string server,
		string api,
		string controller,
		string function,
		string endpoint,
		int code,
		string value
	);
}
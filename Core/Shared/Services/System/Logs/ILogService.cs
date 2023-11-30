using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Shared.Services.System.Logs;

public interface ILogService : IBaseEntityService<Log, DTOLog>
{
	public Task<List<DTOLog>> GetAll();
	public Task<List<DTOLog>> GetRange(int start, int nbItems);
	public Task<List<Log>> GetAllUnsent();
	public Task SendLogs(List<Log> logs, string address);
	public Task ReceiveLogs(List<DTOLog> dtoLogs);
	public Task DeleteAllLogs();

	Task Create(
		DateTimeOffset date,
		string server,
		string username,
		string api,
		string controller,
		string function,
		string endpoint,
		int code,
		string value
		);
}
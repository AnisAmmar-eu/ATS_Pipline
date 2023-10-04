using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Services.System.Logs;

public interface ILogsService
{
	Task<List<DTOLog>> GetAll();

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
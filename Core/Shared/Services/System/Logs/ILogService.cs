using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Shared.Services.SystemApp.Logs;

public interface ILogService : IBaseEntityService<LogEntry, DTOLogEntry>
{
	public Task<List<DTOLogEntry>> GetAll();
	public Task<List<DTOLogEntry>> GetRange(int start, int nbItems);
	public Task<List<LogEntry>> GetAllUnsent();
	public Task SendLogs(List<LogEntry> logs);
	public Task ReceiveLogs(List<DTOLogEntry> dtoLogs);
	public Task DeleteAllLogs();
}
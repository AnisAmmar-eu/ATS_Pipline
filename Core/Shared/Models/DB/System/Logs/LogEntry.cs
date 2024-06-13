using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Models.DB.System.Logs;

public partial class LogEntry : BaseEntity, IBaseEntity<LogEntry, DTOLogEntry>
{
	public string Message { get; set; } = string.Empty;
	public string MessageTemplate { get; set; } = string.Empty;
	public string Level { get; set; } = "Debug";
	public string? Exception { get; set; }
	public string? Properties { get; set; }
	public string Source { get; set; } = string.Empty;
	public string Type { get; set; } = "Message";
	public bool HasBeenSent { get; set; }
}
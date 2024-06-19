using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Models.DTO.System.Logs;

public partial class DTOLogEntry : DTOBaseEntity, IDTO<LogEntry, DTOLogEntry>
{
	public string Message { get; set; } = string.Empty;
	public string MessageTemplate { get; set; } = string.Empty;
	public string Level { get; set; } = string.Empty;
	public string? Exception { get; set; }
	public string? Properties { get; set; }
	public string Source { get; set; } = string.Empty;
	public string? Instance { get; set; }
	public bool HasBeenSent { get; set; }
}
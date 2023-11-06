using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Models.DB.System.Logs;

public partial class Log : BaseEntity, IBaseEntity<Log, DTOLog>
{
	public string? Server { get; set; }
	public string? Api { get; set; }
	public string? Controller { get; set; }
	public string? Function { get; set; }
	public string? Endpoint { get; set; }
	public int? Code { get; set; }
	public string? Value { get; set; }
	public bool HasBeenSent { get; set; }
	public int StationID { get; set; }
}
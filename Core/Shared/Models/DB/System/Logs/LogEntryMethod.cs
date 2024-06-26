using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.System.Logs;
using Mapster;

namespace Core.Shared.Models.DB.System.Logs;

public partial class LogEntry
{
	public LogEntry()
	{
		HasBeenSent = Station.IsServer;
	}

	public override DTOLogEntry ToDTO() => this.Adapt<DTOLogEntry>();
}
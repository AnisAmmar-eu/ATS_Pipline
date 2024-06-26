using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.System.Logs;
using Mapster;

namespace Core.Shared.Models.DTO.System.Logs;

public partial class DTOLogEntry
{
	public DTOLogEntry()
	{
		HasBeenSent = Station.IsServer;
	}

	public override LogEntry ToModel() => this.Adapt<LogEntry>();
}
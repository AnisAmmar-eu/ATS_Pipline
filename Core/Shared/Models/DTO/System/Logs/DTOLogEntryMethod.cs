using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.System.Logs;

namespace Core.Shared.Models.DTO.System.Logs;

public partial class DTOLogEntry
{
	public DTOLogEntry()
	{
		HasBeenSent = Station.IsServer;
	}

	public DTOLogEntry(LogEntry log)
	{
		Message = log.Message;
		MessageTemplate = log.MessageTemplate;
		Level = log.Level;
		Exception = log.Exception;
		Properties = log.Properties;
		Source = log.Source;
		HasBeenSent = log.HasBeenSent;
	}

	public override LogEntry ToModel() => new(this);
}
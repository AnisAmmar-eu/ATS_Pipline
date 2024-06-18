using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.System.Logs;

namespace Core.Shared.Models.DB.System.Logs;

public partial class LogEntry
{
	public LogEntry()
	{
		HasBeenSent = Station.IsServer;
	}

	public LogEntry(DTOLogEntry dto)
	{
		Message = dto.Message;
		MessageTemplate = dto.MessageTemplate;
		Level = dto.Level;
		Exception = dto.Exception;
		Properties = dto.Properties;
		Source = dto.Source;
		Instance = dto.Instance;
		HasBeenSent = dto.HasBeenSent;
	}

	public override DTOLogEntry ToDTO() => new(this);
}
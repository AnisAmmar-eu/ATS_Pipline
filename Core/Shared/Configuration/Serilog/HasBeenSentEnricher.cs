using Core.Shared.Dictionaries;
using Serilog.Core;
using Serilog.Events;

namespace Core.Configuration.Serilog;

public class HasBeenSentEnricher : ILogEventEnricher
{
	public void Enrich(
		LogEvent logEvent,
		ILogEventPropertyFactory propertyFactory)
	{
		LogEventProperty enrichProperty = propertyFactory
			.CreateProperty("HasBeenSent", Station.IsServer);

		logEvent.AddOrUpdateProperty(enrichProperty);
	}
}
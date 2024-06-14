using Core.Shared.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace ApiMonitor.Enrichers;

public class SourceEnricher : ILogEventEnricher
{
	private readonly string _source;

	public SourceEnricher(IConfiguration configuration)
	{
		_source = configuration.GetValueWithThrow<string>("StationConfig:StationName");
	}

	public void Enrich(
		LogEvent logEvent,
		ILogEventPropertyFactory propertyFactory)
	{
		LogEventProperty enrichProperty = propertyFactory
			.CreateProperty("Source", _source);

		logEvent.AddOrUpdateProperty(enrichProperty);
	}
}
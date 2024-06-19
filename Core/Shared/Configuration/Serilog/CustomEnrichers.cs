using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Core.Configuration.Serilog;

public class CustomEnrichers : ILogEventEnricher
{
	private readonly string _source;
	private readonly string? _instance;

	public CustomEnrichers(IConfiguration configuration)
	{
		_source = configuration.GetValueWithThrow<string>("StationConfig:StationName");
		_instance = configuration.GetValue<string>("StationConfig:InstanceName")
			?? System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
	}

	public void Enrich(
		LogEvent logEvent,
		ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("HasBeenSent", Station.IsServer));
		logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Source", _source));
		logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Instance", _instance));
	}
}
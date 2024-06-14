using Serilog.Configuration;
using Serilog;

namespace ApiMonitor.Enrichers;

public static class LoggingExtensions
{
	public static LoggerConfiguration WithCustomEnrichers(
		this LoggerEnrichmentConfiguration enrich, IConfiguration configuration)
			=> (enrich is null)
				? throw new ArgumentNullException(nameof(enrich))
				: enrich.With(new SourceEnricher(configuration))
					.Enrich
					.With<HasBeenSentEnricher>();
}
using Core.Entities.BI.BITemperatures.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.BI.BITemperature;

public class TemperatureService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<TemperatureService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromMinutes(15);
	private int _executionCount;

	public TemperatureService(ILogger<TemperatureService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	private TimeSpan TimeToWaitUntilNextQuarterHour()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		return TimeSpan.FromMinutes(15 - now.Minute % 15);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Delay(TimeToWaitUntilNextQuarterHour(), stoppingToken);
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IBITemperatureService biTemperatureService =
			asyncScope.ServiceProvider.GetRequiredService<IBITemperatureService>();
		using PeriodicTimer timer = new(_period);
		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				_logger.LogInformation("TemperatureService running at: {time}", DateTimeOffset.Now);

				_logger.LogInformation("TemperatureService registering all temperatures.");
				await biTemperatureService.LogNewValues();

				_logger.LogInformation("TemperatureService purging temperatures.");
				await biTemperatureService.PurgeByTimestamp(TimeSpan.FromHours(2));

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicTemperatureService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicTemperatureService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
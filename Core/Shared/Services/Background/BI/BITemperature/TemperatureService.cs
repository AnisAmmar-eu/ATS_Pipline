using Core.Entities.BI.BITemperatures.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.BI.BITemperature;

/// <summary>
/// Background service responsibles for logging both camera temperatures every <see cref="Delay"/> seconds.
/// </summary>
public class TemperatureService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<TemperatureService> _logger;
	private const int Delay = 5;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(Delay);
	private int _executionCount;

	public TemperatureService(ILogger<TemperatureService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	private static TimeSpan TimeToWaitUntilNextQuarterHour()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		return TimeSpan.FromSeconds(Delay - (now.Second % Delay));
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Delay(TimeToWaitUntilNextQuarterHour(), stoppingToken);
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IBITemperatureService biTemperatureService
			= asyncScope.ServiceProvider.GetRequiredService<IBITemperatureService>();
		using PeriodicTimer timer = new(_period);
		do
		{
			try
			{
				await biTemperatureService.LogNewValues();
				await biTemperatureService.RemoveByLifeSpan(TimeSpan.FromHours(2));

				_executionCount++;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PeriodicTemperatureService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		} while (await timer.WaitForNextTickAsync(stoppingToken)
		 && !stoppingToken.IsCancellationRequested);
	}
}
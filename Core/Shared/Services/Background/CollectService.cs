using Core.Entities.AlarmsLog.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class CollectService : BackgroundService
{
	private readonly TimeSpan _period = TimeSpan.FromMilliseconds(100);
	private int _executionCount = 0;
	private readonly ILogger<CollectService> _logger;
	private readonly IServiceScopeFactory _factory;

	public CollectService(ILogger<CollectService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using PeriodicTimer timer = new(_period);

		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
		{
			try
			{
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IAlarmLogService alarmLogService = asyncScope.ServiceProvider.GetRequiredService<IAlarmLogService>();

				_logger.LogInformation("CollectService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling Collect");
				await alarmLogService.Collect();

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicCollectService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicCollectService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		}
	}
}
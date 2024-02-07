using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for sending alarm logs
/// </summary>
public class SendAlarmLogService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<SendAlarmLogService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(5);

	private readonly IConfiguration _configuration;
	private int _executionCount;

	public SendAlarmLogService(
		ILogger<SendAlarmLogService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		int delayMS = _configuration.GetValueWithThrow<int>("SendAlarmCycleMS");
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(delayMS));
		IAlarmLogService alarmLogService
			= asyncScope.ServiceProvider.GetRequiredService<IAlarmLogService>();
		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
        {
            try
			{
				_logger.LogInformation("SendService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling SendAlarmLogs");
				await alarmLogService.SendLogsToServer();

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicSendService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PeriodicSendService with exception message {message}. Good luck next round!",
					ex.Message);
			}
        }
    }
}
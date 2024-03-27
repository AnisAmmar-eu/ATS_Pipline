using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
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

	private readonly IConfiguration _configuration;

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
		int sendAlarmCycleMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.SendAlarmCycleMS);
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(sendAlarmCycleMS));
		IAlarmLogService alarmLogService
			= asyncScope.ServiceProvider.GetRequiredService<IAlarmLogService>();
		IAlarmRTService alarmRTService
			= asyncScope.ServiceProvider.GetRequiredService<IAlarmRTService>();
		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
        {
            try
			{
				await alarmLogService.SendLogsToServer();
				await alarmRTService.SendRTsToServer();
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
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class SendService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<SendService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(10);
	private int _executionCount;

	public SendService(ILogger<SendService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using PeriodicTimer timer = new(_period);
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IStationCycleService stationCycleService =
			asyncScope.ServiceProvider.GetRequiredService<IStationCycleService>();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();

		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				_logger.LogInformation("SendService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling SendStationCycle");
				await stationCycleService.SendStationCycles(await stationCycleService.GetAllReadyToSent(), Station.ServerAddress);

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicSendService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicSendService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
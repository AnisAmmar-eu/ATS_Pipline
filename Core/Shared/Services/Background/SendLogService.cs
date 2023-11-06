using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Services.System.Logs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class SendLogService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<SendLogService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(1);
	private int _executionCount;

	public SendLogService(ILogger<SendLogService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using PeriodicTimer timer = new(_period);
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		ILogService logService =
			asyncScope.ServiceProvider.GetRequiredService<ILogService>();

		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				_logger.LogInformation("SendLogService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling SendLog");
				await logService.SendLogs(await logService.GetAllUnsent(), Station.ServerAddress);

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicSendLogService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicSendLogService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
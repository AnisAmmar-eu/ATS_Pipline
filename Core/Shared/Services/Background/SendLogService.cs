using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for sending station logs to the server.
/// </summary>
public class SendLogService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<SendLogService> _logger;
	private readonly IConfiguration _configuration;

	public SendLogService(ILogger<SendLogService> logger, IServiceScopeFactory factory, IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int sendLogMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.SendLogMS);
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(sendLogMS));
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		ILogService logService
			= asyncScope.ServiceProvider.GetRequiredService<ILogService>();

		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
		{
			try
			{
				// await logService.SendLogs(await logService.GetAllUnsent());
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PeriodicSendLogService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		}
	}
}
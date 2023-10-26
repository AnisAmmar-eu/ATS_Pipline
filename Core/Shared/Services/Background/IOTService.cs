using Core.Entities.IOT.IOTDevices.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class IOTService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<IOTService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(100000);
	private int _executionCount;

	public IOTService(ILogger<IOTService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using PeriodicTimer timer = new(_period);
		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IIOTDeviceService iotDeviceService =
					asyncScope.ServiceProvider.GetRequiredService<IIOTDeviceService>();

				_logger.LogInformation("IOTService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling CheckAllConnectionsAndApplyTags");

				await iotDeviceService.CheckAllConnectionsAndApplyTags();

				_executionCount++;
				_logger.LogInformation("Executed PeriodicIOTService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicIOTService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
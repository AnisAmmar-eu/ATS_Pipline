using System.Configuration;
using Core.Entities.IOT.IOTDevices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class IOTService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<IOTService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(1);
	private int _executionCount;

	public IOTService(ILogger<IOTService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using PeriodicTimer timer = new(_period);
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IIOTDeviceService iotDeviceService
			= asyncScope.ServiceProvider.GetRequiredService<IIOTDeviceService>();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		// If there is no given devices RIDs to monitor, it defaults to monitoring all APIs.
		IEnumerable<string> rids = configuration.GetSection("Devices").Get<string[]>()
			?? await iotDeviceService.GetAllApisRIDs();

		while (!stoppingToken.IsCancellationRequested
			&& await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
			{
				_logger.LogInformation("IOTService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling CheckAllConnectionsAndApplyTags");

				await iotDeviceService.CheckAllConnectionsAndApplyTags(rids);

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
}
using Core.Entities.IOT.IOTDevices.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for monitoring devices & apis statuses and processing their tags.
/// </summary>
public class IOTService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<IOTService> _logger;
	private readonly IConfiguration _configuration;

	public IOTService(ILogger<IOTService> logger, IServiceScopeFactory factory, IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int iOTMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.IOTMS);
		// If there is no given devices RIDs to monitor, it defaults to monitoring all APIs.

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(iOTMS), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
			IIOTDeviceService iotDeviceService
			= asyncScope.ServiceProvider.GetRequiredService<IIOTDeviceService>();
			IEnumerable<string> rids = configuration.GetSection("Devices").Get<string[]>()
				?? await iotDeviceService.GetAllApisRIDs();
			try
			{
				await iotDeviceService.CheckAllConnectionsAndApplyTags(rids);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PeriodicIOTService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		}
	}
}
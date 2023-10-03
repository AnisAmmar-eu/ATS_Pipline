using System.Net.NetworkInformation;
using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Entities.ServicesMonitors.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class MonitorService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<MonitorService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
	private int _executionCount;

	public MonitorService(ILogger<MonitorService> logger, IServiceScopeFactory factory)
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
				IServicesMonitorService servicesMonitorService =
					asyncScope.ServiceProvider.GetRequiredService<IServicesMonitorService>();

				_logger.LogInformation("MonitorService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("Calling UpdateAllStatus");

				await servicesMonitorService.PingAllAndUpdate();

				_executionCount++;
				_logger.LogInformation("Executed PeriodicMonitorService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicMonitorService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
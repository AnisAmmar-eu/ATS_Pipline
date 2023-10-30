using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRTs;

public class BaseHourlyKPIRTService<T, TDTO, TService, TValue> : BackgroundService
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>, IBaseKPI<TValue>
	where TService : class, IServiceBaseEntity<T, TDTO>
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<BaseHourlyKPIRTService<T, TDTO, TService, TValue>> _logger;
	private readonly TimeSpan _period = TimeSpan.FromHours(1);
	private int _executionCount;

	public BaseHourlyKPIRTService(IServiceScopeFactory factory,
		ILogger<BaseHourlyKPIRTService<T, TDTO, TService, TValue>> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	private TimeSpan TimeToWaitUntilNextHour()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		return TimeSpan.FromMinutes(60 - now.Minute);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Delay(TimeToWaitUntilNextHour());
		using PeriodicTimer timer = new(_period);
		do
		{
			try
			{
				_logger.LogInformation("BaseKPIRTService running at: {time}", DateTimeOffset.Now);

				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				TService tService =
					asyncScope.ServiceProvider.GetRequiredService<TService>();
				IKPIRTService kpirtService =
					asyncScope.ServiceProvider.GetRequiredService<IKPIRTService>();

				_logger.LogInformation("Calling ComputeKPIRTs");
				await kpirtService.ComputeKPIRTs<T, TDTO, TService, TValue>(tService);

				_executionCount++;
				_logger.LogInformation("Executed BaseKPIRTService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute BaseKPIRTService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		} while (!stoppingToken.IsCancellationRequested
		         && await timer.WaitForNextTickAsync(stoppingToken));
	}
}
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Background.KPI.KPILog;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRT;

public class BaseKPIRTService<T, TDTO, TService> : BackgroundService
	where T : class, IBaseEntity<T, TDTO>, IBaseKPI
	where TDTO : class, IDTO<T, TDTO>
	where TService : class, IServiceBaseEntity<T, TDTO>
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<BaseKPIRTService<T, TDTO, TService>> _logger;
	private readonly TimeSpan _period = TimeSpan.FromHours(24);
	private int _executionCount;

	public BaseKPIRTService(IServiceScopeFactory factory, ILogger<BaseKPIRTService<T, TDTO, TService>> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	private TimeSpan TimeToWaitUntilMidnight()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		DateTimeOffset nextDayStart = DateTimeOffset.Now.DateTime.Date.AddDays(1);
		return nextDayStart - now;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Delay(TimeToWaitUntilMidnight());
		using PeriodicTimer timer = new(_period);
		do
			try
			{
				_logger.LogInformation("BaseKPIRTService running at: {time}", DateTimeOffset.Now);

				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				TService tService =
					asyncScope.ServiceProvider.GetRequiredService<TService>();

				_executionCount++;
				_logger.LogInformation("Executed BaseKPIRTService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute BaseKPIRTService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken));
	}
}
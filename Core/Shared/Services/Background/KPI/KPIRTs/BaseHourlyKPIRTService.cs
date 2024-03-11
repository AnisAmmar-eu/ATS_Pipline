using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRTs;

/// <summary>
/// Background service responsible for computing KPIRT values every hour.
/// </summary>
/// <typeparam name="T">Entities on which the computation will be done.</typeparam>
/// <typeparam name="TDTO"></typeparam>
/// <typeparam name="TRepository"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class BaseHourlyKPIRTService<T, TDTO, TRepository, TValue> : BackgroundService
	where T : class, IBaseEntity<T, TDTO>, IBaseKPI<TValue>
	where TDTO : class, IDTO<T, TDTO>
	where TRepository : class, IBaseEntityRepository<T, TDTO>
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<BaseHourlyKPIRTService<T, TDTO, TRepository, TValue>> _logger;
	private readonly TimeSpan _period = TimeSpan.FromHours(1);
	private int _executionCount;

	public BaseHourlyKPIRTService(
		IServiceScopeFactory factory,
		ILogger<BaseHourlyKPIRTService<T, TDTO, TRepository, TValue>> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	private static TimeSpan TimeToWaitUntilNextHour()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		return TimeSpan.FromMinutes(60 - now.Minute);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await Task.Delay(TimeToWaitUntilNextHour(), stoppingToken);
		using PeriodicTimer timer = new(_period);
		do
		{
			try
			{
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
				IKPIRTService kpirtService = asyncScope.ServiceProvider.GetRequiredService<IKPIRTService>();

				await kpirtService.ComputeKPIRTs<T, TDTO, TRepository, TValue>(
					(anodeUOW.GetRepoByType(typeof(TRepository)) as TRepository)!);

				_executionCount++;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute BaseKPIRTService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		} while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested);
	}
}
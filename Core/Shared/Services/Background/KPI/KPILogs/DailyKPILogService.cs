using Core.Entities.KPI.KPIEntries.Dictionaries;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPILogs;

/// <summary>
/// Background service responsible for logging every KPIRT every day at midnight.
/// </summary>
public class DailyKPILogService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<DailyKPILogService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromHours(24);
	private int _executionCount;

	public DailyKPILogService(IServiceScopeFactory factory, ILogger<DailyKPILogService> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	private static TimeSpan TimeToWaitUntilMidnight()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		DateTimeOffset nextDayStart = DateTimeOffset.Now.DateTime.Date.AddDays(1);
		return nextDayStart - now;
	}

	private static List<string> GetPeriodsList()
	{
		DateTimeOffset now = DateTimeOffset.Now;
		List<string> periods = [KPIPeriod.Day];
		if (now.DayOfWeek == DayOfWeek.Monday)
			periods.Add(KPIPeriod.Week);

		if (now.Day == 1)
			periods.Add(KPIPeriod.Month);

		if (now.DayOfYear == 1)
			periods.Add(KPIPeriod.Year);

		return periods;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// It is run at 0h05 to allow KPIRT to be computed.
		await Task.Delay(TimeToWaitUntilMidnight() + TimeSpan.FromMinutes(5), stoppingToken);
		using PeriodicTimer timer = new(_period);
		do
		{
			bool retry = true;
			while (retry)
            {
                try
				{
					List<string> periods = GetPeriodsList();

					await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
					IKPIRTService kpirtService
						= asyncScope.ServiceProvider.GetRequiredService<IKPIRTService>();

					List<DTOKPILog> dtoKPILogs = await kpirtService.SaveRTsToLogs(periods);

					_executionCount++;
					retry = false;
				}
				catch (Exception ex)
				{
					_logger.LogError(
						"Failed to execute DailyKPILogService with exception message {message}. Good luck next round!",
						ex.Message);
				}
            }
        } while (!stoppingToken.IsCancellationRequested
         && await timer.WaitForNextTickAsync(stoppingToken));
	}
}
using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Services;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class ModeDebugService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<ModeDebugService> _logger;

	public ModeDebugService(ILogger<ModeDebugService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			IDebugModeService debugModeService = asyncScope.ServiceProvider.GetRequiredService<IDebugModeService>();

			try
			{
				List<DebugMode> settings = await anodeUOW.DebugMode.GetAll();

				if (settings is not null)
				{
					foreach (DebugMode debugMode in settings)
					{
						await debugModeService.ApplyDebugMode(debugMode.DebugModeEnabled);
						debugModeService.ApplyCsvExport(debugMode.CsvExportEnabled);
						debugModeService.ApplyLog(debugMode.LogEnabled, debugMode.LogSeverity);
					}
				}

				await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed to execute ModeDebugService: {Message}", ex.Message);
			}
		}
	}
}
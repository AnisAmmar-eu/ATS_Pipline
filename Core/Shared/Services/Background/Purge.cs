using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.BI.BITemperatures.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.TwinCat;
using Core.Shared.Services.Background.BI.BITemperature;
using Core.Shared.Services.System.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

public class PurgeService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<PurgeService> _logger;
    private readonly IConfiguration _configuration;

    public PurgeService(
        ILogger<PurgeService> logger,
        IServiceScopeFactory factory,
        IConfiguration configuration)
    {
        _logger = logger;
        _factory = factory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

        int purgeThreshold = _configuration.GetValueWithThrow<int>("PurgeThreshold");
        int purgeTimer = _configuration.GetValueWithThrow<int>("PurgeTimer");
        using PeriodicTimer timer = new (TimeSpan.FromSeconds(purgeTimer));

        IAlarmLogService alarmLogService
    = asyncScope.ServiceProvider.GetRequiredService<IAlarmLogService>();

        ILogService logService
    = asyncScope.ServiceProvider.GetRequiredService<ILogService>();

        IAlarmCService alarmCService
    = asyncScope.ServiceProvider.GetRequiredService<IAlarmCService>();

        IPacketService paquetService
    = asyncScope.ServiceProvider.GetRequiredService<IPacketService>();

        while (await timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("PurgeService running at: {time}", DateTimeOffset.Now);

				_logger.LogInformation("alarmLogService purging.");
                await alarmLogService.RemoveByLifeSpan(TimeSpan.FromSeconds(purgeThreshold));

                _logger.LogInformation("alarmCService purging.");
                await alarmCService.RemoveByLifeSpan(TimeSpan.FromSeconds(purgeThreshold));

                _logger.LogInformation("paquetService purging.");
                await paquetService.RemoveByLifeSpan(TimeSpan.FromSeconds(purgeThreshold));

                _logger.LogInformation("logService purging.");
                await logService.RemoveByLifeSpan(TimeSpan.FromSeconds(purgeThreshold));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to execute PurgeService with exception message {message}.",
                    ex.Message);
            }
        }
    }
}
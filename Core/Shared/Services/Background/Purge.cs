using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.BI.BITemperatures.Services;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
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
                DateTimeOffset threshold = DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(purgeThreshold));

                _logger.LogInformation("alarmLogService purging.");
                List<AlarmLog> alarmLogs = (await alarmLogService.GetAll())
                    .Where(paquet => paquet.TS < threshold)
                    .ToList()
                    .ConvertAll(alarmLog => alarmLog.ToModel());

                foreach (AlarmLog alarmLog in alarmLogs)
                {
                    if (alarmLog.HasBeenSent)
                        await alarmLogService.Remove(alarmLog);
                }

                _logger.LogInformation("paquetService with images and alarmCycle purging.");
                List<AlarmC> alarmC = (await alarmCService.GetAll()).ConvertAll(alarm => alarm.ToModel());
                List<Packet> paquets = (await paquetService.GetAll()).Where(paquet => paquet.TS < threshold).ToList()
                    .ConvertAll(paquet => paquet.ToModel());

                foreach (Packet paquet in paquets)
                {
                    if (paquet.Status != PacketStatus.Sent)
                    {
                        paquets.Remove(paquet);
                        continue;
                    }

                    if (paquet is Shooting)
                    {
                        FileInfo thumbnail1 = await paquetService.GetThumbnailFromCycleRIDAndCamera(
                            paquet.StationCycleRID,
                            1);
                        FileInfo thumbnail2 = await paquetService.GetThumbnailFromCycleRIDAndCamera(
                            paquet.StationCycleRID,
                            2);

                        FileInfo image1 = await paquetService.GetImageFromCycleRIDAndCamera(
                            paquet.StationCycleRID,
                            1);
                        FileInfo image2 = await paquetService.GetImageFromCycleRIDAndCamera(
                            paquet.StationCycleRID,
                            2);

                        if (File.Exists(image1.FullName))
                            File.Delete(image1.FullName);

                        if (File.Exists(image2.FullName))
                            File.Delete(image2.FullName);

                        if (File.Exists(thumbnail1.FullName))
                            File.Delete(thumbnail1.FullName);

                        if (File.Exists(thumbnail2.FullName))
                            File.Delete(thumbnail2.FullName);
                    }

                    if (paquet is AlarmList)
                        await alarmCService.Remove(alarmC.Find(alarm => alarm.RID == paquet.StationCycleRID));
                }

                await paquetService.RemoveAll(paquets);

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
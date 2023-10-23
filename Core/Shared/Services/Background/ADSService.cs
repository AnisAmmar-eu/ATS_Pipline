using System.Dynamic;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Notifications;
using Core.Shared.Services.Notifications.PacketNotifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

public class ADSService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<ADSService> _logger;
	private int _executionCount;

	public ADSService(ILogger<ADSService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		CancellationToken cancel = CancellationToken.None;
		AdsClient tcClient = new();
		while (!stoppingToken.IsCancellationRequested)
			try
			{
				_logger.LogInformation("ADSService running at: {time}", DateTimeOffset.Now);
				while (true)
				{
					tcClient.Connect(851);
					if (tcClient.IsConnected) break;
					Console.WriteLine("Unable to connect to the automaton. Retrying in 1 second");
					Thread.Sleep(1000);
				}

				dynamic ads = new ExpandoObject();
				ads.tcClient = tcClient;
				ads.appServices = asyncScope.ServiceProvider;
				ads.cancel = cancel;
				_logger.LogInformation("Calling Notifications");
				await AnnouncementNotification.Create(ads);
				await DetectionNotification.Create(ads);
				await AlarmNotification.Create(ads);
				if (Station.Type == StationType.S3S4)
				{
					await InFurnaceNotification.Create(ads);
					await OutFurnaceNotification.Create(ads);
				}
				_logger.LogInformation("ADSService successfully created Notifications");

				// If the TC disconnects, it will loop back to the top.
				uint handle = tcClient.CreateVariableHandle(ADSUtils.AnnouncementNewMsg);
				while ((await tcClient.ReadAnyAsync<bool>(handle, cancel)).ErrorCode ==
				       AdsErrorCode.NoError)
					// To avoid spamming the TwinCat
					Thread.Sleep(1000);
				
				_logger.LogInformation("PeriodicADSService lost connection to the TwinCat");
				
				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicADSService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicADSService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
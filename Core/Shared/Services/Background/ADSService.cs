using System.Dynamic;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Core.Shared.Services.Notifications;
using Core.Shared.Services.Notifications.PacketNotifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT;
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

		while (!stoppingToken.IsCancellationRequested)
        {
            try
			{
				AdsClient tcClient = await InitializeConnection(asyncScope, cancel);
				// If the TC disconnects, it will loop back to the top
				uint handle = tcClient.CreateVariableHandle(ADSUtils.ConnectionPath);
				while (!stoppingToken.IsCancellationRequested)
                {
                    if ((await tcClient.ReadAnyAsync<bool>(handle, cancel)).ErrorCode != AdsErrorCode.NoError)
						throw new AdsException("Error while reading variable");

                    await Task.Delay(1000, cancel);
                }
            }
			catch (Exception e)
			{
				_logger.LogInformation(
					"PeriodicADSService lost connection with path {ConnectionPath} to the TwinCat with error message: {error}",
					ADSUtils.ConnectionPath,
					e);

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicADSService - Count: {count}", _executionCount);
			}
        }
    }

	private async Task<AdsClient> InitializeConnection(AsyncServiceScope asyncScope, CancellationToken cancel)
	{
		try
		{
			_logger.LogInformation("ADSService running at: {time}", DateTimeOffset.Now);

			AdsClient tcClient = await TwinCatConnectionManager.Connect(851, cancel);
			dynamic ads = new ExpandoObject();
			ads.tcClient = tcClient;
			ads.appServices = asyncScope.ServiceProvider;
			ads.cancel = cancel;
			_logger.LogInformation("Calling Notifications");
			await AnnouncementNotification.Create(ads);
			await AlarmNotification.Create(ads);
			if (Station.Type == StationType.S3S4)
			{
				await InFurnaceNotification.Create(ads);
				await OutFurnaceNotification.Create(ads);
			}

			_logger.LogInformation("ADSService successfully created Notifications");
			return tcClient;
		}
		catch (Exception ex)
		{
			_logger.LogInformation(
				"Failed to execute PeriodicADSService with exception message {message}. Good luck next round!",
				ex.Message);
			throw;
		}
	}
}
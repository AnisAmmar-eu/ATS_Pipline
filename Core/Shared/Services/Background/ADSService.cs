using System.Configuration;
using System.Dynamic;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Core.Shared.Services.Notifications;
using Core.Shared.Services.Notifications.PacketNotifications;
using Microsoft.Extensions.Configuration;
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

		// Assign configuration
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		string? imagesPath = configuration.GetValue<string>("CameraConfig:ImagesPath");
		if (imagesPath is null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ImagesPath");

		string? thumbnailsPath = configuration.GetValue<string>("CameraConfig:ThumbnailsPath");
		if (thumbnailsPath is null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ThumbnailsPath");

		AssignNotification.ImagesPath = imagesPath;
		AssignNotification.ThumbnailsPath = thumbnailsPath;

		while (!stoppingToken.IsCancellationRequested)
        {
            try
			{
				AdsClient tcClient = await InitializeConnection(asyncScope, cancel);
				// If the TC disconnects, it will loop back to the top
				uint handle = tcClient.CreateVariableHandle(ADSUtils.AnnouncementNewMsg);
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
					"PeriodicADSService lost connection to the TwinCat with error message: {error}",
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

			AdsClient tcClient = TwinCatConnectionManager.Connect(851);
			dynamic ads = new ExpandoObject();
			ads.tcClient = tcClient;
			ads.appServices = asyncScope.ServiceProvider;
			ads.cancel = cancel;
			_logger.LogInformation("Calling Notifications");
			await AnnouncementNotification.Create(ads);
			await DetectionNotification.Create(ads);
			await AlarmNotification.Create(ads);

			await AssignNotification.Create(ads);
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
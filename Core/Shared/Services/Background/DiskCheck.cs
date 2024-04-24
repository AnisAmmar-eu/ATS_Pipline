using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

public class DiskCheckService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<DiskCheckService> _logger;
	private readonly IConfiguration _configuration;

	public DiskCheckService(
		ILogger<DiskCheckService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration
		)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		int cycleMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.CycleMS);
		long diskCheckThreshold = _configuration.GetValueWithThrow<long>(ConfigDictionary.DiskCheckThreshold);
		int retryMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.RetryMS);
		string diskCheckLabel = _configuration.GetValueWithThrow<string>(ConfigDictionary.DiskCheckLabel);
		CancellationToken cancel = CancellationToken.None;
		_logger.LogInformation(
			"DiskCheckService started with cycleMS {cycleMS}"
				+ "and diskCheckThreshold {diskCheckThreshold}MB",
			cycleMS,
			diskCheckThreshold);
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(cycleMS));

		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
		{
			try
			{
				foreach (DriveInfo d in DriveInfo.GetDrives())
				{
					if (d.Name.Equals(diskCheckLabel + ":\\", StringComparison.Ordinal) && d.IsReady)
					{
						// only returns free space of current user
						long freeSpaceMB = d.AvailableFreeSpace / (1024 * 1024);
						_logger.LogInformation(
							"{freeSpace}MB free space left for Drive {driveName}",
							freeSpaceMB,
							d.Name);

						AdsClient tcClient = await TwinCatConnectionManager.Connect(
							851,
							_logger,
							retryMS,
							cancel);
						uint alarmTimeHandle = tcClient.CreateVariableHandle(ADSUtils.DiskSpace);
						tcClient.WriteAny(alarmTimeHandle, (freeSpaceMB <= diskCheckThreshold) ? 2 : 1);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute DiskCheckService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		}
	}
}
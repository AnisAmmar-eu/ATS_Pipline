using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background
{
	public class DiskCheckService : BackgroundService
	{
		private readonly IServiceScopeFactory _factory;
		private readonly ILogger<SendAlarmLogService> _logger;
		private readonly IConfiguration _configuration;

		public DiskCheckService(
			ILogger<SendAlarmLogService> logger,
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
			int checkTimer = _configuration.GetValueWithThrow<int>("DiskCheckTimer");
			long checkThreshold= _configuration.GetValueWithThrow<long>("DiskCheckThreshold");
			int retryMS = _configuration.GetValueWithThrow<int>("RetryMS");
			CancellationToken cancel = CancellationToken.None;

			using PeriodicTimer timer = new(TimeSpan.FromSeconds(checkTimer));

			while (await timer.WaitForNextTickAsync(stoppingToken)
				&& !stoppingToken.IsCancellationRequested)
			{
				try
				{
					foreach (DriveInfo d in DriveInfo.GetDrives())
					{
						// Check only local physical storage mount
						if (!d.DriveType.ToString().Equals("Fixed"))
							continue;

						if (d.IsReady)
						{
							// only returns free space of current user
							long freeSpaceMB = d.AvailableFreeSpace / 1024*1024;
							string msg = string.Format(
								"{0}MB free space left for Drive {1}",
								freeSpaceMB,
								d.VolumeLabel);

							AdsClient tcClient = await TwinCatConnectionManager.Connect(
								851,
								_logger,
								retryMS,
								cancel);
							uint alarmTimeHandle = tcClient.CreateVariableHandle(ADSUtils.DiskSpace);

							if (freeSpaceMB <= checkThreshold)
							{
								_logger.LogCritical(msg);
								tcClient.WriteAny(alarmTimeHandle, 2);
								continue;
							}

							_logger.LogInformation(msg);
							tcClient.WriteAny(alarmTimeHandle, 1);
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
}
using System.Dynamic;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Configuration;
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

/// <summary>
/// Background service responsible for starting the NotificationServices used to retrieve data from the automaton SQL queues.
/// </summary>
public class ADSService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<ADSService> _logger;
	private readonly IConfiguration _configuration;
	private int _executionCount;

	public ADSService(ILogger<ADSService> logger, IServiceScopeFactory factory, IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	/// <summary>
	/// Executes an asynchronous task that continuously monitors and processes ADS notifications.
	/// This task creates and manages different types of notifications (Alarm, Shooting, InFurnace, OutFurnace)
	/// and performs actions based on these notifications.
	/// </summary>
	/// <remarks>
	/// This method uses a <see cref="PeriodicTimer"/> to periodically execute tasks. It creates ADS services
	/// for different notifications and retrieves elements asynchronously based on the station type.
	/// The method handles exceptions by logging information and increments an execution count.
	/// </remarks>
	/// <param name="stoppingToken">A <see cref="CancellationToken"/> that signals the method to stop its execution.</param>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IAlarmRTService alarmRTService = asyncScope.ServiceProvider.GetRequiredService<IAlarmRTService>();
		CancellationToken cancel = CancellationToken.None;
		int delayMS = _configuration.GetValueWithThrow<int>("CycleMS");
		int retryMS = _configuration.GetValueWithThrow<int>("RetryMS");
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(delayMS));

		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Create ADSService");
			try
			{
				AdsClient tcClient = await TwinCatConnectionManager.Connect(851, _logger, retryMS, cancel);
				/*
					Create each notification :
					AlarmNotification
					InFurnaceNotification
					OutFurnaceNotification
				*/
				(uint alarmNewmsg, uint alarmOldEntry) = CreateVarADS(
					tcClient,
					ADSUtils.AlarmNewMsg,
					ADSUtils.AlarmToRead);
				AlarmNotification alarmNotification = new(alarmNewmsg, alarmOldEntry, _logger);

				uint alarmListHandle = tcClient.CreateVariableHandle(ADSUtils.AlarmList);

				(uint inFurnaceNewMsg, uint inFurnaceOldEntry) = CreateVarADS(
					tcClient,
					ADSUtils.InFurnaceNewMsg,
					ADSUtils.InFurnaceToRead);
				InFurnaceNotification inFurnaceNotification = new(inFurnaceNewMsg, inFurnaceOldEntry, _logger);

				(uint outFurnaceNewMsg, uint outFurnaceOldEntry) = CreateVarADS(
					tcClient,
					ADSUtils.OutFurnaceNewMsg,
					ADSUtils.OutFurnaceToRead);
				OutFurnaceNotification outFurnaceNotification = new(outFurnaceNewMsg, outFurnaceOldEntry, _logger);

				_logger.LogInformation("ADS Loop started");
				// Define a task that retrieves all the elements asynchronously
				Task GetAllElements()
				{
					return Task.WhenAll(
						alarmNotification.GetElement(tcClient, asyncScope.ServiceProvider),
						alarmRTService.Collect(tcClient, alarmListHandle),
						(Station.Type == StationType.S3S4)
							? Task.WhenAll(
								inFurnaceNotification.GetElement(tcClient, asyncScope.ServiceProvider),
								outFurnaceNotification.GetElement(tcClient, asyncScope.ServiceProvider))
							: Task.CompletedTask);
				}

				uint handle = tcClient.CreateVariableHandle(ADSUtils.ConnectionPath);
				while (await timer.WaitForNextTickAsync(stoppingToken)&& !stoppingToken.IsCancellationRequested)
				{
					if ((await tcClient.ReadAnyAsync<bool>(handle, cancel)).ErrorCode != AdsErrorCode.NoError)
						throw new AdsException("Error while reading variable");

					_logger.LogInformation("Time to get elements");
					await GetAllElements();
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

	/// <summary>
	/// Creates msgNew and OldEntry variable handles for the notification services and returns them.
	/// </summary>
	/// <param name="tcClient">The AdsClient instance.</param>
	/// <param name="newMsgPath">The path of the new message variable.</param>
	/// <param name="oldEntryPath">The path of the old entry variable.</param>
	private static (uint newMsgHandle, uint oldEntryHandle) CreateVarADS(
		AdsClient tcClient, string newMsgPath, string oldEntryPath)
	{
		uint newMsgHandle =  tcClient.CreateVariableHandle(newMsgPath);
		uint oldEntryHandle =  tcClient.CreateVariableHandle(oldEntryPath);

		return (newMsgHandle, oldEntryHandle);
	}
}
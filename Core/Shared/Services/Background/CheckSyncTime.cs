using System.Text.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.TwinCat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for sending alarm logs
/// </summary>
public class CheckSyncTimeService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<CheckSyncTimeService> _logger;

	private readonly IConfiguration _configuration;

	public CheckSyncTimeService(
		ILogger<CheckSyncTimeService> logger,
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
		int checkSyncTimeMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.CheckSyncTimeMS);
		int deltaTimeSec = _configuration.GetValueWithThrow<int>(ConfigDictionary.DeltaTimeSec);
		int retryMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.RetryMS);
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(checkSyncTimeMS));

		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
		{
			try
			{
				string api2Url = $"{ITApisDict.ServerReceiveAddress}/apiServerReceive/time";
				CancellationToken cancel = CancellationToken.None;
				await Task.Run(
					async () => {
						using HttpClient httpClient = new();
						HttpResponseMessage response = await httpClient.GetAsync(api2Url, cancel);

						if (!response.IsSuccessStatusCode)
						{
							throw new("Get time to server failed with status code:"
								+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
						}

						// Delta Time station and server Check
						ApiResponse? apiResponse
						= JsonSerializer.Deserialize<ApiResponse>(await response.Content.ReadAsStreamAsync())
							?? throw new ApplicationException("Could not deserialize ApiIOT response");
						if (apiResponse.Result is not JsonElement jsonElement)
							throw new ApplicationException("JSON Exception, ApiResponse from ApiIOT is broken");

						DateTimeOffset serverTime = jsonElement.Deserialize<DateTimeOffset>();
						DateTimeOffset stationTime = DateTimeOffset.Now;
						_logger.LogInformation(
							"Server time is {serverTime}, station time is {stationTime}",
							serverTime,
							stationTime);
						TimeSpan delta = serverTime - stationTime;
						_logger.LogInformation("Delta time between server and station is {delta}", delta);
						AdsClient tcClient = await TwinCatConnectionManager.Connect(851, _logger, retryMS, cancel);
						// Trigger an alarm
						uint alarmTimeHandle = tcClient.CreateVariableHandle(ADSUtils.SyncTime);
						_logger.LogInformation("Delt IS superior ? {delta}", Math.Abs(delta.TotalSeconds) <= deltaTimeSec);
						if (Math.Abs(delta.TotalSeconds) <= deltaTimeSec)
							tcClient.WriteAny(alarmTimeHandle, 1);
						else
							tcClient.WriteAny(alarmTimeHandle, 2);
					}
					,
					cancel);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PeriodicSendService with exception message {message}. Good luck next round!",
					ex.Message);
			}
		}
	}
}
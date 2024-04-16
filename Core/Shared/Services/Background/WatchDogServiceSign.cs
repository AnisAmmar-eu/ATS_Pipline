using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for watching over the signs and updating their watchdog time.
/// </summary>
public class WatchDogServiceSign : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<WatchDogServiceSign> _logger;

	private readonly IConfiguration _configuration;

	public WatchDogServiceSign(
		ILogger<WatchDogServiceSign> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int retryMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.RetryMS);
		int stationID = Station.StationNameToID(
			_configuration.GetValueWithThrow<string>(ConfigDictionary.StationName)
		);
		string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(retryMS), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			try
			{
				List<Sign> signes = (await _anodeUOW.IOTDevice.GetAll([device => device is Sign]))
					.Cast<Sign>()
					.Where(sign => sign.StationID == stationID && sign.AnodeType == anodeType)
					.ToList();

				if (signes.Count == 0)
					throw new EntityNotFoundException(nameof(Sign), stationID);

				foreach (Sign sign in signes)
					sign.WatchdogTime = DateTimeOffset.Now;

				await _anodeUOW.StartTransaction();
				_anodeUOW.Sign.UpdateArray([.. signes]);
				_anodeUOW.Commit();
				await _anodeUOW.CommitTransaction();
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
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.Vision;

/// <summary>
/// Background service responsible for watching over the Match instances and updating their WatchdogTime.
/// </summary>
public class WatchDogServiceMatch : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<WatchDogServiceMatch> _logger;

	private readonly IConfiguration _configuration;

	public WatchDogServiceMatch(
		ILogger<WatchDogServiceMatch> logger,
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
		int instanceMatchID = _configuration.GetValueWithThrow<int>(ConfigDictionary.InstanceMatchID);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(retryMS), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			try
			{
				List<Match> matches = (await _anodeUOW.IOTDevice.GetAll([device => device is Match]))
					.Cast<Match>()
					.Where(match => match.InstanceMatchID == instanceMatchID)
					.ToList();

				if (matches.Count == 0)
					throw new EntityNotFoundException(nameof(Match), instanceMatchID);

				foreach (Match match in matches)
					match.WatchdogTime = DateTimeOffset.Now;

				await _anodeUOW.StartTransaction();
				_anodeUOW.Match.UpdateArray([.. matches]);
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
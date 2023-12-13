using System.Configuration;
using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Services.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Services.MatchableStacks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.Vision;

public class LoadMatchService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<LoadMatchService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromDays(1);

	public LoadMatchService(IServiceScopeFactory factory, ILogger<LoadMatchService> logger)
	{
		_factory = factory;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();

		TimeSpan s3S4Delay = TimeSpan.Parse(configuration.GetValue<string>("VisionSettings:S3S4Delay")
			?? throw new ConfigurationErrorsException("Missing VisionSettings:S3S4Delay"));
		TimeSpan s5Delay = TimeSpan.Parse(configuration.GetValue<string>("VisionSettings:S5Delay")
			?? throw new ConfigurationErrorsException("Missing VisionSettings:S5Delay"));

		ILoadableQueueService loadableQueueService
			= asyncScope.ServiceProvider.GetRequiredService<ILoadableQueueService>();
		IMatchableStackService matchableStackService
			= asyncScope.ServiceProvider.GetRequiredService<IMatchableStackService>();
		using PeriodicTimer timer = new(_period);
		do
		{
			try
			{
				_logger.LogInformation("LoadMatchService: Starting next load...");
				LoadableQueue?[] loadables = await loadableQueueService.LoadNextCycles(
					[(DataSetID.S3S4, s3S4Delay),
					(DataSetID.S5, s5Delay),
				]);
				_logger.LogInformation("LoadMatchService: Loading completed, onto matching");

				_logger.LogInformation("LoadMatchService: Starting next matching...");
				await matchableStackService.MatchNextCycles([
					(DataSetID.S3S4, s3S4Delay, loadables[0]),
					(DataSetID.S5, s5Delay, loadables[1]),
				]);
				_logger.LogInformation("LoadMatchService: Matching completed, onto next iteration...");
			}
			catch (Exception e)
			{
				_logger.LogError("Error in LoadMatchService: {error}", e);
			}
		} while (!stoppingToken.IsCancellationRequested
			&& await timer.WaitForNextTickAsync(stoppingToken));
	}
}
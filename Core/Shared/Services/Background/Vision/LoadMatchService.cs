using System.Configuration;
using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
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
	private readonly TimeSpan _period = TimeSpan.FromSeconds(1);

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
		while (!stoppingToken.IsCancellationRequested
			&& await timer.WaitForNextTickAsync(stoppingToken))
		{
			try
			{
				_logger.LogInformation("LoadMatchService: Starting next load...");
				LoadableQueue? s3S4Loadable = await loadableQueueService.Peek(DataSetID.S3S4);
				LoadableQueue? s5Loadable = await loadableQueueService.Peek(DataSetID.S5);
				Task t1 = loadableQueueService.LoadNextCycle(s3S4Loadable, s3S4Delay);
				Task t2 = loadableQueueService.LoadNextCycle(s5Loadable, s5Delay);
				await t1;
				await t2;
				_logger.LogInformation("LoadMatchService: Loading completed, onto matching");

				_logger.LogInformation("LoadMatchService: Starting next matching...");
				MatchableStack? s3S4Matchable = await matchableStackService.Peek(DataSetID.S3S4);
				MatchableStack? s5Matchable = await matchableStackService.Peek(DataSetID.S5);
				t1 = matchableStackService.MatchNextCycle(s3S4Matchable, s3S4Loadable, s3S4Delay);
				t2 = matchableStackService.MatchNextCycle(s5Matchable, s5Loadable, s5Delay);
				await t1;
				await t2;
				_logger.LogInformation("LoadMatchService: Matching completed, onto next iteration...");
			}
			catch (Exception e)
			{
				_logger.LogError("Error in LoadMatchService: {error}", e);
			}
		}
	}
}
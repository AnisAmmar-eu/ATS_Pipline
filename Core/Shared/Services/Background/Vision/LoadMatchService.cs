using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Services.ToLoads;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
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
	{/*
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();

		TimeSpan s3S4Delay = TimeSpan.Parse(configuration.GetValueWithThrow<string>(ConfigDictionary.S3S4Delay));
		TimeSpan s5Delay = TimeSpan.Parse(configuration.GetValueWithThrow<string>(ConfigDictionary.S5Delay));

		ToLoadService ToLoad
			= asyncScope.ServiceProvider.GetRequiredService<ToLoadService>();
		IToSignService ToMatch
			= asyncScope.ServiceProvider.GetRequiredService<IToSignService>();
		using PeriodicTimer timer = new(_period);
		do
		{
			try
			{
				ToLoad?[] loadables = await ToLoad.LoadNextCycles(
					[(DataSetID.S3S4, s3S4Delay),
					(DataSetID.S5, s5Delay),
				]);

				await ToMatch.MatchNextCycles([
					(DataSetID.S3S4, s3S4Delay, loadables[0]),
					(DataSetID.S5, s5Delay, loadables[1]),
				]);
			}
			catch (Exception e)
			{
				_logger.LogError("Error in LoadMatchService: {error}", e);
			}
		} while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested);
		*/
	}
}
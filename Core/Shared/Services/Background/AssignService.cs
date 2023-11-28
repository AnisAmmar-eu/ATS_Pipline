using System.Configuration;
using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Shared.Services.Background;

public class AssignService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<AssignService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromMilliseconds(100);
	private int _executionCount;

	public AssignService(ILogger<AssignService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IStationCycleService stationCycleService =
			asyncScope.ServiceProvider.GetRequiredService<IStationCycleService>();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		using PeriodicTimer timer = new(_period);
		string? imagesPath = configuration.GetValue<string>("CameraConfig:ImagesPath");
		if (imagesPath == null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ImagesPath");
		string? thumbnailsPath = configuration.GetValue<string>("CameraConfig:ThumbnailsPath");
		if (thumbnailsPath == null)
			throw new ConfigurationErrorsException("Missing CameraConfig:ThumbnailsPath");

		AdsClient tcClient = TwinCatConnectionManager.Connect(851);
		uint closeCycleHandle = tcClient.CreateVariableHandle(ADSUtils.CloseCycle);
		uint detectionHandle = tcClient.CreateVariableHandle(ADSUtils.DetectionToRead);
		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				if (!(await tcClient.ReadAnyAsync<bool>(closeCycleHandle, stoppingToken)).Value)
					continue;

				_logger.LogInformation("AssignService running at: {time}", DateTimeOffset.Now);

				_logger.LogInformation("AssignService calling AssignStationCycle");

				//await stationCycleService.AssignStationCycle(tcClient, detectionHandle, imagesPath, thumbnailsPath);

				await tcClient.WriteAnyAsync(closeCycleHandle, false, stoppingToken);
				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicAssignService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicAssignService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}
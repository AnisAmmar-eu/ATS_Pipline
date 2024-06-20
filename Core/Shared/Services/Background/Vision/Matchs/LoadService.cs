using System.Data;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.DLLVision;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.Vision.Matchs;

public class LoadService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<LoadService> _logger;
	private readonly IConfiguration _configuration;

	public LoadService(
		ILogger<LoadService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	/// <summary>Load in dataset</summary>
	/// <summary>Need to get all ToLoad with InstanceMatchID</summary>
	/// <summary>Then use DLLVision to load</summary>
	/// <summary>Update dataset having add loaded anode line</summary>
	/// <summary>Then delete load line from Load table</summary>
	/// <param name="stoppingToken">Cancellation token</param>
	/// <returns>Task</returns>
	/// <exception cref="Exception">Failed to execute LoadService with exception message {message}.</exception>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		double stationDelay = _configuration.GetValueWithThrow<double>(ConfigDictionary.StationDelay);
		int instanceMatchID = _configuration.GetValueWithThrow<int>(ConfigDictionary.InstanceMatchID);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(signMatchTimer), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			try
			{
				Match match = (Match)await anodeUOW.IOTDevice
					.GetByWithThrow(
						[device => device is Match && ((Match)device).InstanceMatchID == instanceMatchID],
						withTracking: false);

				ServerRule rule = (ServerRule)await anodeUOW.IOTDevice
					.GetByWithThrow(
						[device => device is ServerRule],
						withTracking: false);

				if (match.Pause || rule.Reinit)
					continue;

				ToLoad? toLoad = await anodeUOW.ToLoad.GetBy(
					[load => load.InstanceMatchID == instanceMatchID],
					orderBy: query => query.OrderByDescending(
						toLoad => toLoad.ShootingTS),
					withTracking: false);

				if (toLoad is null)
					continue;

				StationCycle cycle = await anodeUOW.StationCycle.GetById(toLoad.StationCycleID);
				if (cycle.TSFirstShooting?.AddDays(stationDelay) > DateTimeOffset.Now)
					break;

				FileInfo sanFile = Shooting.GetImagePathFromRoot(
					toLoad.CycleRID,
					toLoad.StationID,
					imagesPath,
					toLoad.AnodeType,
					toLoad.CameraID,
					extension);

				int loadResponse = DLLVisionImport.fcx_load_anode(
					toLoad.CameraID,
					sanFile.DirectoryName ?? string.Empty,
					Path.GetFileNameWithoutExtension(sanFile.Name));

				if (loadResponse != 0)
				{
					_logger.LogError(
						"Failed to unload anode with response code {responseCode}.",
						loadResponse);
					continue;
				}

				Dataset dataset = toLoad.Adapt<Dataset>();
				await anodeUOW.Dataset.Add(dataset);
				anodeUOW.Commit();

				await anodeUOW.ToLoad.ExecuteDeleteAsync(
					load => load.ID == toLoad.ID);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute LoadService with exception message {message}.",
					ex.Message);
			}
		}
	}
}
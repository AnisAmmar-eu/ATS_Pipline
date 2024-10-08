﻿using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class PurgeServer : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<PurgeService> _logger;
	private readonly IConfiguration _configuration;

	public PurgeServer(
		ILogger<PurgeService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int purgeThresholdSec = _configuration.GetValueWithThrow<int>(ConfigDictionary.PurgeThreshold);
		int purgeTimerSec = _configuration.GetValueWithThrow<int>(ConfigDictionary.PurgeTimerSec);
		int purgeRawPictures = _configuration.GetValueWithThrow<int>(ConfigDictionary.PurgeRawPictures);
		int purgeMetadata = _configuration.GetValueWithThrow<int>(ConfigDictionary.PurgeMetadata);
		int purgeAnodeEntry = _configuration.GetValueWithThrow<int>(ConfigDictionary.PurgeAnodeEntry);
		int purgeCycle = _configuration.GetValueWithThrow<int>(ConfigDictionary.PurgeCycle);
		IEnumerable<IConfigurationSection> purgeDatasetInstances = _configuration.GetSection(
			ConfigDictionary.PurgeDatasetInstances)
			.GetChildren();

		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		TimeSpan purgeThreshold = TimeSpan.FromSeconds(purgeThresholdSec);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(purgeTimerSec), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			try
			{
				_logger.LogInformation("PurgeService running at: {time}", DateTimeOffset.Now);
				_logger.LogError("PurgeService threshold: {threshold}", purgeThreshold.ToString());

				DateTimeOffset threshold = DateTimeOffset.Now.Subtract(purgeThreshold);
				_logger.LogError("PurgeService threshold date: {threshold}", threshold.ToString());

				DateTimeOffset purgeRaw = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(purgeRawPictures));
				List<StationCycle> cycles = await anodeUOW.StationCycle
					.GetAll([t => t.TS < purgeRaw]);

				foreach (StationCycle cycle in cycles)
				{
					// Delete image
					string cycleRID = cycle.RID;
					string anodeType = cycle.AnodeType;
					int stationID = cycle.StationID;

					FileInfo image1 = Shooting.GetImagePathFromRoot(cycleRID, stationID, imagesPath, anodeType, 1, extension);
					FileInfo image2 = Shooting.GetImagePathFromRoot(cycleRID, stationID, imagesPath, anodeType, 2, extension);

					DeleteFileIfExists(image1);
					DeleteFileIfExists(image2);
				}

				// Delete AlarmLog
				await anodeUOW.AlarmLog.ExecuteDeleteAsync(alarmLog => alarmLog.TS < threshold && alarmLog.HasBeenSent);

				// Delete Log
				await anodeUOW.Logs.RemoveByLifeSpan(purgeThreshold);

				// Delete Metadata (12 mois)
				TimeSpan span = TimeSpan.FromDays(purgeMetadata);
				await anodeUOW.StationCycle.RemoveByLifeSpan(span);
				await anodeUOW.Packet.RemoveByLifeSpan(span);

				//Delete  Entry (5 years)
				await anodeUOW.Anode.RemoveByLifeSpan(TimeSpan.FromDays(purgeAnodeEntry));

				//Incomplete cycle ( 6 month )
				DateTimeOffset incompleteAnodeThreshold = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(purgeCycle));
				await anodeUOW.Anode.ExecuteDeleteAsync(anode => anode.TS < incompleteAnodeThreshold && !anode.IsComplete);

				// Delete Datasets
				foreach (IConfigurationSection purgeDatasetInstance in purgeDatasetInstances)
				{
					if (double.TryParse(purgeDatasetInstance.Value, out double value))
					{
						IEnumerable<int> instanceMatchIDs = (await anodeUOW.Match
							.GetAll([match => match.Family == purgeDatasetInstance.Key]))
							.ConvertAll(match => match.InstanceMatchID)
							.Distinct();
						DateTimeOffset purgeDatasetTS = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(value));

						List<Dataset> datasets = (await anodeUOW.Dataset.GetAll(
							[data => instanceMatchIDs.Contains(data.InstanceMatchID) && data.TS < purgeDatasetTS]
							));
						if (datasets.Count > 0)
						{
							anodeUOW.ToUnload.AddRange(datasets
								.DistinctBy(x => x.CycleRID)
								.Select(x => x.Adapt<ToUnload>()));
							anodeUOW.Dataset.RemoveRange(datasets);
						}
					}
				}

				anodeUOW.Commit();
				await anodeUOW.CommitTransaction();
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PurgeService with exception message {message}.",
					ex.Message);
			}
		}
	}

	private static void DeleteFileIfExists(FileInfo file)
	{
		try
		{
			if (file.Exists)
				file.Delete();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to delete file {file.FullName} with exception message {ex.Message}.");
		}
	}
}
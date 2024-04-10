using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class PurgeService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<PurgeService> _logger;
	private readonly IConfiguration _configuration;
	private IAnodeUOW _anodeUOW;

	public PurgeService(
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
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string thumbnailsPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ThumbnailsPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		TimeSpan purgeThreshold = TimeSpan.FromSeconds(purgeThresholdSec);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(purgeTimerSec), stoppingToken);
            await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
            _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
            try
			{
				_logger.LogInformation("PurgeService running at: {time}", DateTimeOffset.Now);
				_logger.LogError("PurgeService threshold: {threshold}", purgeThreshold.ToString());

				DateTimeOffset threshold = DateTimeOffset.Now.Subtract(purgeThreshold);
				_logger.LogError("PurgeService threshold date: {threshold}", threshold.ToString());

				// Delete AlarmLog
				await _anodeUOW.AlarmLog.ExecuteDeleteAsync(alarmLog => alarmLog.TS < threshold && alarmLog.HasBeenSent);

				// Delete Log
				await _anodeUOW.Log.RemoveByLifeSpan(purgeThreshold);

				// Delete Packet
				List<Packet> packets = await _anodeUOW.Packet.GetAll(
					[paquet => paquet.TS < threshold && paquet.Status == PacketStatus.Sent],
					withTracking: false);

				foreach (Packet packet in packets)
				{
					_logger.LogError("PurgeService packet: {packet}", packet.ID);
					// Delete images
					if (packet is Shooting shooting)
					{
						string cycleRID = shooting.StationCycleRID;
						string anodeType = shooting.AnodeType;
						int stationID = Station.ID;

						FileInfo thumbnail1 = Shooting.GetImagePathFromRoot(cycleRID, stationID, thumbnailsPath, anodeType, 1, extension);
						FileInfo thumbnail2 = Shooting.GetImagePathFromRoot(cycleRID, stationID, thumbnailsPath, anodeType, 2, extension);
						FileInfo image1 = Shooting.GetImagePathFromRoot(cycleRID, stationID, imagesPath, anodeType, 1, extension);
						FileInfo image2 = Shooting.GetImagePathFromRoot(cycleRID, stationID, imagesPath, anodeType, 2, extension);

						DeleteFileIfExists(thumbnail1);
						DeleteFileIfExists(thumbnail2);
						DeleteFileIfExists(image1);
						DeleteFileIfExists(image2);
					}

					// Delete AlarmCycle
					if (packet is AlarmList alarmList)
						await _anodeUOW.AlarmCycle.ExecuteDeleteAsync(alarm => alarm.AlarmListPacketID == alarmList.ID);
				}

				await _anodeUOW.StartTransaction();
				_anodeUOW.Packet.RemoveRange(packets);
				_anodeUOW.Commit();
				await _anodeUOW.CommitTransaction();
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
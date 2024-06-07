using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Core.Entities.Vision.Testing.Models.DB;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Dictionaries;

namespace Core.Shared.Services.Background.Vision;

public class StationFeedService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<StationFeedService> _logger;
	private readonly IConfiguration _configuration;

	public StationFeedService(
		ILogger<StationFeedService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		int cycleMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.CycleMS);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(cycleMS), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

			await anodeUOW.StartTransaction();

			try
			{
				List<StationTest> stationTests = await anodeUOW.StationTest.GetAll([test => test.ShootingTS < DateTimeOffset.Now]);

				foreach (StationTest test in stationTests)
				{
					MetaData metaData = new() { Status = PacketStatus.Completed };
					Shooting shooting1 = new() { Status = PacketStatus.Completed };
					Shooting shooting2 = new() { Status = PacketStatus.Completed };

					metaData = test.MapMetadata(metaData);
					shooting1 = test.MapShooting(shooting1);
					shooting2 = test.MapShooting(shooting2);

					shooting1.Cam02Status = 0;
					shooting2.Cam01Status = 0;

					anodeUOW.Packet.Add(metaData);
					anodeUOW.Packet.Add(shooting1);
					anodeUOW.Packet.Add(shooting2);

					for (int i = 1; i <= 2; i++)
					{
						FileInfo image = Shooting.GetImagePathFromRoot(
							metaData.StationCycleRID,
							test.StationID,
							imagesPath,
							shooting1.AnodeType,
							i,
							extension);

						if (image.DirectoryName is not null)
							Directory.CreateDirectory(image.DirectoryName);

						if (i == 1)
							File.Copy(test.Photo1, image.FullName, true);
						else
							File.Copy(test.Photo2, image.FullName, true);
					}

					await anodeUOW.StationTest.ExecuteDeleteAsync(x => stationTests.Contains(x));

					anodeUOW.Commit();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			await anodeUOW.CommitTransaction();
		}
	}
}
using Core.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Core.Shared.Dictionaries;
using DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Mapster;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;

namespace Core.Shared.Services.Background.Vision;

public class MatchService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<MatchService> _logger;
	private readonly IConfiguration _configuration;

	public MatchService(
		ILogger<MatchService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		string _imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string _extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		int instanceMatchID = _configuration.GetValueWithThrow<int>(ConfigDictionary.InstanceMatchID);
		string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
		int stationDelay = _configuration.GetValueWithThrow<int>(ConfigDictionary.StationDelay);
		bool isChained = _configuration.GetValueWithThrow<bool>(ConfigDictionary.IsChained);
		List<string> stationOrigins = _configuration.GetSectionWithThrow<List<string>>(ConfigDictionary.GoMatchStations);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(signMatchTimer), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			IToMatchService toMatchService
		   = asyncScope.ServiceProvider.GetRequiredService<IToMatchService>();

			await _anodeUOW.StartTransaction();

			try
			{
				List<ToMatch> toMatchs = await _anodeUOW.ToMatch.GetAll(
					[match => match.InstanceMatchID == instanceMatchID && match.AnodeType == anodeType],
					withTracking: false);

				foreach (ToMatch toMatch in toMatchs)
				{
					// if (!await toMatchService.GoMatch(stationOrigins, toMatch.IntanceMatchID, stationDelay))
					// continue;
					_logger.LogInformation("debut de matching {cycleRID}", toMatch.CycleRID);

					_anodeUOW.ToMatch.Remove(toMatch);
					_anodeUOW.Commit();

					for (int cameraID = 1; cameraID <= 2; cameraID++)
					{
						MatchableCycle cycle = (MatchableCycle)await _anodeUOW.StationCycle.GetById(toMatch.StationCycleID);
						if (cycle.TSFirstShooting?.AddDays(stationDelay) > DateTimeOffset.Now)
							break;

						FileInfo image = Shooting.GetImagePathFromRoot(
							toMatch.CycleRID,
							toMatch.StationID,
							_imagesPath,
							toMatch.AnodeType,
							cameraID,
							_extension);

						IntPtr retMatch = DLLVisionImport.fcx_match(
							cameraID,
							0,
							image.DirectoryName,
							Path.GetFileNameWithoutExtension(image.Name));
						int matchErrorCode = DLLVisionImport.fcx_matchRet_errorCode(retMatch);

						if (matchErrorCode == 0 || matchErrorCode == -106)
						{
							_logger.LogInformation("{nb} matché avec code d'erreur {error}", image.Name, matchErrorCode);
							cycle = await toMatchService.UpdateCycle(cycle, retMatch, cameraID, isChained);

							if (matchErrorCode == 0)
							{
								if (!isChained)
									toMatchService.UpdateAnode(cycle);

								await _anodeUOW.ToUnload.Add(toMatch.Adapt<ToUnload>());
								break; //either camera has matched successfully, not need to go further
							}
						}
						else
						{
							_logger.LogWarning("Return code de la signature: " + retMatch + " pour anode " + image.Name);
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute MatchService with exception message {message}.",
					ex.Message);
			}

			await _anodeUOW.CommitTransaction();
		}
	}
}
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Entities.Vision.ToDos.Services.ToUnloads;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using System.Runtime.InteropServices;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;

namespace Core.Shared.Services.Background.Vision.Matchs;

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
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		int instanceMatchID = _configuration.GetValueWithThrow<int>(ConfigDictionary.InstanceMatchID);
		double stationDelay = _configuration.GetValueWithThrow<double>(ConfigDictionary.StationDelay);
		bool isChained = _configuration.GetValueWithThrow<bool>(ConfigDictionary.IsChained);
		List<string> stationOrigins = _configuration.GetSectionWithThrow<List<string>>(ConfigDictionary.GoMatchStations);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await Task.Delay(TimeSpan.FromSeconds(signMatchTimer), stoppingToken);
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
				IToMatchService toMatchService
					= asyncScope.ServiceProvider.GetRequiredService<IToMatchService>();

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

				ToMatch? toMatch = await anodeUOW.ToMatch.GetBy(
					[match => match.InstanceMatchID == instanceMatchID],
					query => query.OrderBy(x => x.ShootingTS));

				if (toMatch is null
					|| !await toMatchService.GoMatch(stationOrigins, instanceMatchID, stationDelay, toMatch.ShootingTS))
				{
					continue;
				}

				anodeUOW.StartTransaction();

				_logger.LogInformation("debut de matching {cycleRID}", toMatch.CycleRID);

				anodeUOW.ToMatch.Remove(toMatch);
				anodeUOW.Commit();

				foreach (int cameraID in new int[] { 1, 2 })
				{
					MatchableCycle cycle = (MatchableCycle)await anodeUOW.StationCycle.GetById(toMatch.StationCycleID);

					FileInfo image = Shooting.GetImagePathFromRoot(
						toMatch.CycleRID,
						toMatch.StationID,
						imagesPath,
						toMatch.AnodeType,
						cameraID,
						extension);

					nint retMatch = DLLVisionImport.fcx_match(
						cameraID,
						1,
						image.DirectoryName ?? string.Empty,
						Path.GetFileNameWithoutExtension(image.Name));
					int matchErrorCode = DLLVisionImport.fcx_matchRet_errorCode(retMatch);
					string? anodeID = Marshal.PtrToStringAnsi(DLLVisionImport.fcx_matchRet_anodeId(retMatch));
					_logger.LogInformation(
						"{nb} matché avec code d'erreur {error}",
						anodeID,
						matchErrorCode);

					if (matchErrorCode == 0 || matchErrorCode == -106)
					{
						cycle = await toMatchService.UpdateCycle(cycle, retMatch, cameraID, isChained);
						int retfree = DLLVisionImport.fcx_matchRet_free(retMatch);

						if (matchErrorCode == 0)
						{
							string? cycleRID = Shooting.GetCycleRIDFromFilename(anodeID!);

							if (!isChained)
							{
								await toMatchService.UpdateAnode(cycle, cycleRID);
								// Get S1S2Cycle and Add ToNotify row
								string imagePath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
								FileInfo file = Shooting.GetImagePathFromFilename(imagePath, $"{anodeID!}.{extension}");
								StationCycle? cycleMatched = await anodeUOW.StationCycle.GetBy([cycle => cycle.RID == cycleRID]);

								if (cycleMatched is not null)
								{
									ToNotify toNotify = new() {
										StationID = cycleMatched.StationID,
										SynchronisationKey = cycleMatched.RID,
										ShootingTS = cycleMatched.TSFirstShooting ?? DateTimeOffset.Now,
										SerialNumber = cycleMatched.SerialNumber,
										Path = file.FullName,
									};

									anodeUOW.ToNotify.Add(toNotify);
									anodeUOW.Commit();
								}
							}
							else
							{
								await toMatchService.UpdateChainedCycle(cycle, cycleRID);
							}

							foreach (int instance in await ToUnloadService.GetInstances(instanceMatchID, anodeUOW))
							{
								ToUnload toUnload = toMatch.Adapt<ToUnload>();
								toUnload.InstanceMatchID = instance;
								toUnload.CycleRID = cycleRID;
								await anodeUOW.ToUnload.Add(toUnload);
							}

							anodeUOW.Commit();
							break; //either camera has matched successfully, not need to go further
						}
					}
					else
					{
						_logger.LogWarning("Return code de la signature: {retMatch} pour anode {image}", matchErrorCode, image.Name);
					}
				}

				anodeUOW.CommitTransaction();
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute MatchService with exception message {message}.",
					ex.Message);
			}
		}

		_logger.LogError("End of match service");
	}
}
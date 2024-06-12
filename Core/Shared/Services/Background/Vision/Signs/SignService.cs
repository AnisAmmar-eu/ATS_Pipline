using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Services.ToLoads;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.Vision.Signs;

public class SignService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<SignService> _logger;
	private readonly IConfiguration _configuration;

	public SignService(
		ILogger<SignService> logger,
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
		List<string> loadDestinations = _configuration.GetSectionWithThrow<List<string>>(
			ConfigDictionary.LoadDestinations);
		string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);
		Station.Name = _configuration.GetValueWithThrow<string>(ConfigDictionary.StationName);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(signMatchTimer), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			IToSignService toSignService
				= asyncScope.ServiceProvider.GetRequiredService<IToSignService>();
			await anodeUOW.StartTransaction();

			try
			{
				Sign sign = (Sign)await anodeUOW.IOTDevice
					.GetByWithThrow(
						[device => device is Sign && ((Sign)device).StationID == Station.ID && ((Sign)device).AnodeType == anodeType],
						withTracking: false);

				ServerRule rule = (ServerRule)await anodeUOW.IOTDevice
					.GetByWithThrow(
						[device => device is ServerRule],
						withTracking: false);

				if (sign.Pause || rule.Reinit)
				{
					_logger.LogWarning("System on pause");
					continue;
				}

				List<ToSign> toSigns = await anodeUOW.ToSign.GetAll(
					[sign => sign.StationID == Station.ID && sign.AnodeType == anodeType],
					withTracking: false);

				foreach (ToSign toSign in toSigns)
				{
					_logger.LogInformation("debut de signature {cycleRID}", toSign.CycleRID);

					FileInfo image = Shooting.GetImagePathFromRoot(
						toSign.CycleRID,
						toSign.StationID,
						imagesPath,
						toSign.AnodeType,
						toSign.CameraID,
						extension);

					string noExtension = Path.GetFileNameWithoutExtension(image.Name);
					int retSign = DLLVisionImport.fcx_sign(
						0,
						toSign.CameraID,
						image.DirectoryName ?? string.Empty,
						noExtension,
						image.DirectoryName ?? string.Empty);

					StationCycle cycle = await toSignService.UpdateCycle(toSign, retSign);

					if (retSign == 0)
					{
						_logger.LogInformation("{nb} signé avec succès", image.Name);

						foreach (string family in loadDestinations)
						{
							foreach (int instanceMatchID in await ToLoadService.GetInstances(family, anodeUOW))
							{
								ToLoad load = toSign.Adapt<ToLoad>();
								load.InstanceMatchID = instanceMatchID;
								await anodeUOW.ToLoad.Add(load);
							}
						}

						// only one match has to be added
						if (cycle.CanMatch())
						{
							foreach (int instanceMatchID in await ToMatchService.GetMatchInstance(
								toSign.AnodeType,
								toSign.StationID,
								anodeUOW))
							{
								ToMatch toMatch = toSign.Adapt<ToMatch>();
								toMatch.InstanceMatchID = instanceMatchID;
								await anodeUOW.ToMatch.Add(toMatch);
							}
						}
					}
					else
					{
						_logger.LogWarning("Return code de la signature: {retSign} pour anode {imageName}", retSign, image.Name);
					}

					anodeUOW.ToSign.Remove(toSign);
					anodeUOW.Commit();

					// S1 and S2 (sign stations) are the only station to add an Anode
					if (!Station.IsMatchStation(cycle.StationID))
						await toSignService.AddAnode((S1S2Cycle)cycle);

					anodeUOW.Commit();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute SignService with exception message {message}.",
					ex.Message);
			}

			await anodeUOW.CommitTransaction();
		}
	}
}
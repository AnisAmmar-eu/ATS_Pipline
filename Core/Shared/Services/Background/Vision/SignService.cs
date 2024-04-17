using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Packets.Models.DB.Shootings;
using Mapster;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Services.ToLoads;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;

namespace Core.Shared.Services.Background.Vision;

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
		string _imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string _extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		List<string> LoadDestinations = _configuration.GetSectionWithThrow<List<string>>(
			ConfigDictionary.LoadDestinations);
		string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(signMatchTimer), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
			IToSignService toSignService
				= asyncScope.ServiceProvider.GetRequiredService<IToSignService>();
			await _anodeUOW.StartTransaction();

			try
			{
				ServerRule rule = (ServerRule)await _anodeUOW.IOTDevice
					.GetByWithThrow([device => device is ServerRule], withTracking: false);

				if (rule.Reinit)
					throw new("Reinit is launched");

				List<ToSign> toSigns = await _anodeUOW.ToSign.GetAll(
					[sign => sign.StationID == Station.ID && sign.AnodeType == anodeType],
					withTracking: false);

				foreach (ToSign toSign in toSigns)
				{
					_logger.LogInformation("debut de signature {cycleRID}", toSign.CycleRID);

					FileInfo image = Shooting.GetImagePathFromRoot(
						toSign.CycleRID,
						toSign.StationID,
						_imagesPath,
						toSign.AnodeType,
						toSign.CameraID,
						_extension);

					string noExtension = Path.GetFileNameWithoutExtension(image.Name);
					int retSign = DLLVisionImport.fcx_sign(
						0,
						toSign.CameraID,
						image.DirectoryName,
						noExtension,
						image.DirectoryName);

					StationCycle cycle = await toSignService.UpdateCycle(toSign, retSign);

					if (retSign == 0)
					{
						_logger.LogInformation("{nb} signé avec succès", image.Name);

						foreach (string family in LoadDestinations)
						{
							foreach (int instanceMatchID in await ToLoadService.GetInstances(family, _anodeUOW))
							{
								ToLoad load = toSign.Adapt<ToLoad>();
								load.InstanceMatchID = instanceMatchID;
								await _anodeUOW.ToLoad.Add(load);
								_anodeUOW.Commit();
							}
						}

						if (cycle.CanMatch())
						{
							ToMatch toMatch = toSign.Adapt<ToMatch>();
							toMatch.InstanceMatchID = await ToMatchService.GetMatchInstance(toSign.AnodeType, toSign.StationID, _anodeUOW);
							await _anodeUOW.ToMatch.Add(toMatch);
						}
					}
					else
					{
						_logger.LogWarning("Return code de la signature: " + retSign + " pour anode " + image.Name);
					}

					_anodeUOW.ToSign.Remove(toSign);
					_ = _anodeUOW.Commit();

					// S1 and S2 (sign stations) are the only station to add an Anode
					if (!Station.IsMatchStation(cycle.StationID))
						toSignService.AddAnode((S1S2Cycle)cycle);

					_ = _anodeUOW.Commit();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute SignService with exception message {message}.",
					ex.Message);
			}

			await _anodeUOW.CommitTransaction();
		}
	}
}
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.InteropServices;
using DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Packets.Models.DB.Shootings;
using System.Configuration;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Mapster;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Vision.ToDos.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;

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
        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
        IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

		string _imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string _extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
        List<InstanceMatchID> LoadDestinations = _configuration.GetSectionWithThrow<List<InstanceMatchID>>(
               ConfigDictionary.LoadDestinations);
        string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
        int cameraID = _configuration.GetValueWithThrow<int>(ConfigDictionary.CameraID);

        int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);
		using PeriodicTimer timer = new (TimeSpan.FromSeconds(signMatchTimer));

		IToSignService toSignService
	   = asyncScope.ServiceProvider.GetRequiredService<IToSignService>();

        TypeAdapterConfig<ToSign, ToLoad>.NewConfig()
               .Ignore(dest => dest.ID);
        TypeAdapterConfig<ToSign, ToMatch>.NewConfig()
               .Ignore(dest => dest.ID);

        while (await timer.WaitForNextTickAsync(stoppingToken)
               && !stoppingToken.IsCancellationRequested)
        {
			await _anodeUOW.StartTransaction();

			try
			{
                List<ToSign> toSigns = await _anodeUOW.ToSign.GetAll(
                    [sign => sign.StationID == Station.ID && sign.CameraID == cameraID && sign.AnodeType == anodeType],
                    withTracking: false);

				foreach (ToSign toSign in toSigns)
				{
                    _logger.LogInformation("debut de signature {0}", toSign.CycleRID);

                    FileInfo image = Shooting.GetImagePathFromRoot(
                        toSign.CycleRID,
                        toSign.StationID,
                        _imagesPath,
                        toSign.AnodeType,
                        toSign.CameraID,
                        _extension);

					string noExtension = image.Name[..^4];
                    int retSign = DLLVisionImport.fcx_sign(0, 0, image.DirectoryName, noExtension, image.DirectoryName);

					if (retSign == 0)
						_logger.LogInformation("{0} signé avec succès", image.Name);
					else
						_logger.LogWarning("Return code de la signature: " + retSign + " pour anode " + image.Name);

					_anodeUOW.ToSign.Remove(toSign);
					_anodeUOW.Commit();

                    StationCycle cycle = await toSignService.UpdateCycle(toSign, retSign);

                    foreach (InstanceMatchID id in LoadDestinations)
					{
						ToLoad load = toSign.Adapt<ToLoad>();
						load.InstanceMatchID = id;
						await _anodeUOW.ToLoad.Add(load);
						_anodeUOW.Commit();
					}

                    // S1 and S2 (sign stations) are the only station to add an Anode
                    if (toSign.IsMatchStation())
                        await _anodeUOW.ToMatch.Add(toSign.Adapt<ToMatch>());
                    else
                        toSignService.AddAnode((S1S2Cycle)cycle);

                    _anodeUOW.Commit();
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
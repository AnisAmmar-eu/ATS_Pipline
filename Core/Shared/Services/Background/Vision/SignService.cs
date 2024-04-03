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
	private IAnodeUOW _anodeUOW;
	private string _imagesPath;
	private string _extension;

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
        _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

        string DLLPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);
        string signStaticParams = _configuration.GetValueWithThrow<string>(ConfigDictionary.SignStaticParams);
        string signDynamicParams = _configuration.GetValueWithThrow<string>(ConfigDictionary.SignDynParams);
        bool allowSignMatch = _configuration.GetValueWithThrow<bool>(ConfigDictionary.AllowSignMatch);
		_imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		_extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);
		using PeriodicTimer timer = new (TimeSpan.FromSeconds(signMatchTimer));

		IToSignService toSignService
	   = asyncScope.ServiceProvider.GetRequiredService<IToSignService>();

		DLLVisionImport.SetDllDirectory(DLLPath);

        int retInit = DLLVisionImport.fcx_init();
        int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams);
        int signParamsDynOutput = DLLVisionImport.fcx_register_sign_params_dynamic(0, signDynamicParams);

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
                    [sign => sign.StationID == Station.ID],
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
					StationCycle cycle = await toSignService.UpdateCycle(toSign, retSign);
					_anodeUOW.Commit();

					foreach (InstanceMatchID id in toSign.GetLoadDestinations())
					{
						ToLoad load = toSign.Adapt<ToLoad>();
						load.DataSetID = id;
						await _anodeUOW.ToLoad.Add(load);
						_anodeUOW.Commit();
					}

                    if (toSign.IsMatchStation())
                        await _anodeUOW.ToMatch.Add(toSign.Adapt<ToMatch>());
                    else if (toSign.AnodeType == AnodeTypes.DX)
                        await _anodeUOW.Anode.Add(new AnodeDX((S1S2Cycle)cycle));
                    else if (toSign.AnodeType == AnodeTypes.D20)
                        await _anodeUOW.Anode.Add(new AnodeDX((S1S2Cycle)cycle));

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
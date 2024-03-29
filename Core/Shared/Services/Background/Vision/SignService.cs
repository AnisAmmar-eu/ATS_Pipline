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
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.Anodes.Models.DB.AnodesD20;

namespace Core.Shared.Services.Background.Vision;

public class SignService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<SignService> _logger;
    private readonly IConfiguration _configuration;
	private readonly IAnodeUOW _anodeUOW;
	private  string _imagesPath;
	private string _extension;

	public SignService(
		ILogger<SignService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration,
		IAnodeUOW anodeUOW)
    {
        _logger = logger;
        _factory = factory;
        _configuration = configuration;
        _anodeUOW = anodeUOW;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

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

        while (await timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                List<ToSign> toSigns = await _anodeUOW.ToSign.GetAll(
                             [sign => sign.StationID == Station.ID],
                             withTracking: false);

				foreach (ToSign toSign in toSigns)
				{
					FileInfo image = Shooting.GetImagePathFromRoot(
						toSign.CycleRID,
						toSign.StationID,
						_imagesPath,
						toSign.AnodeType,
						toSign.CameraID,
						_extension);

					int retSign = DLLVisionImport.fcx_sign(0, 0, image.DirectoryName, image.Name, image.DirectoryName);

					if (retSign == 0)
					{
						_logger.LogInformation("{0} signé avec succès", image.Name);
					}
					else
					{
						_logger.LogWarning(
							"Return code de la signature: " + retSign + " pour anode " + image.Name);
					}

					_anodeUOW.ToSign.Remove(toSign);

					foreach (DataSetID id in toSign.GetDestinations())
					{
						ToLoad load = toSign.Adapt<ToLoad>();
						load.DataSetID = id;
						await _anodeUOW.ToLoad.Add(load);
					}

					StationCycle cycle = await toSignService.UpdateCycle(toSign, retSign);
					if (toSign.AnodeType == AnodeTypes.DX)
						await _anodeUOW.Anode.Add((toSign, cycle).Adapt<AnodeDX>());
					else if (toSign.AnodeType == AnodeTypes.D20)
						await _anodeUOW.Anode.Add((toSign, cycle).Adapt<AnodeD20>());
				}
			}
			catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to execute SignService with exception message {message}.",
                    ex.Message);
            }
        }
    }

}
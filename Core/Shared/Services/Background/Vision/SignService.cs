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
using Core.Entities.Vision.ToDos.Mapper;

namespace Core.Shared.Services.Background.Vision;

public class SignService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<SignService> _logger;
    private readonly IConfiguration _configuration;
	private readonly IAnodeUOW _anodeUOW;

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
        int stationID = Station.StationNameToID(_configuration.GetValueWithThrow<string>(ConfigDictionary.StationName));
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);
		using PeriodicTimer timer = new (TimeSpan.FromSeconds(signMatchTimer));

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
                             [sign => sign.StationID == stationID],
                             withTracking: false);

                List<ToSign> results = new();

                foreach (ToSign toSign in toSigns)
                {
                    FileInfo image = Shooting.GetImagePathFromRoot(
                                    toSign.CycleRID,
                                    toSign.StationID,
                                    imagesPath,
                                    toSign.AnodeType,
                                    toSign.CameraID,
                                    extension);

					if (allowSignMatch)
                    {
						int retSign = DLLVisionImport.fcx_sign(0, 0, image.DirectoryName, image.Name, image.DirectoryName);

						if (retSign == 0)
						{
							_logger.LogInformation(
								"{0} signé avec succès", image.Name);
                            results.Add(toSign);
						}
						else
						{
							_logger.LogWarning(
								"Return code de la signature: " + retSign + " pour anode " + image.Name);
						}
					}
				}

				_anodeUOW.ToSign.RemoveRange(results);
				ToDoMapper mapper = new();
				await _anodeUOW.ToLoad.AddRange(results.ConvertAll(mapper.SignToLoad));
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
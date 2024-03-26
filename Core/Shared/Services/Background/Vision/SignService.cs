using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.InteropServices;
using DLLVision;

namespace Core.Shared.Services.Background.Vision;

public class SignService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<PurgeService> _logger;
    private readonly IConfiguration _configuration;

    public SignService(
        ILogger<PurgeService> logger,
        IServiceScopeFactory factory,
        IConfiguration configuration)
    {
        _logger = logger;
        _factory = factory;
        _configuration = configuration;
    }

    const string image_folder = "C:\\d\\ADSVision\\images";
    const string signature_folder = "C:\\d\\ADSVision\\sign";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

        string DLLPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);
        string signStaticParams = _configuration.GetValueWithThrow<string>(ConfigDictionary.SignStaticParams);
        string signDynamicParams = _configuration.GetValueWithThrow<string>(ConfigDictionary.SignDynParams);
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
                foreach (string image in Directory.GetFiles(image_folder, "*.jpg"))
                {
                    string filename = new FileInfo(image).Name;
                    string anodeId = Path.GetFileNameWithoutExtension(filename);
                    int returnSigner = DLLVisionImport.fcx_sign(0, 0, image_folder, anodeId, signature_folder);
                    _logger.LogInformation("Return code de la signature: " + returnSigner + " pour anode " + anodeId);
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
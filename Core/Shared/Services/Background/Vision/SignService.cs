using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.BI.BITemperatures.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.TwinCat;
using Core.Shared.Services.Background.BI.BITemperature;
using Core.Shared.Services.System.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwinCAT.Ads;

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

	const string sig_static_params_file = "C:\\d\\ADSVision\\DLLVision\\dll\\ConfigStatic\\sign_params_static_01.xml";

    const string sig_dynamic_params_file
        = "C:\\d\\ADSVision\\apialarms\\ApiSign\\dll\\ConfigDynamic\\signature_dynamic_CT.xml";

    const string image_folder = "C:\\d\\ADSVision\\images";
    const string signature_folder = "C:\\d\\ADSVision\\sign";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

        int signTimer = _configuration.GetValueWithThrow<int>("SignTimer");
        using PeriodicTimer timer = new (TimeSpan.FromSeconds(signTimer));

        int retInit = DLLVisionImport.fcx_init();
        int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, sig_static_params_file);
        int signParamsDynOutput = DLLVisionImport.fcx_register_sign_params_dynamic(0, sig_dynamic_params_file);

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
                    _logger.LogInformation("Return code de la signature: " + returnSigner);
                }
			}
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to execute PurgeService with exception message {message}.",
                    ex.Message);
            }
        }
    }
}
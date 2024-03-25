﻿using Core.Shared.Configuration;
using Core.Shared.DLLVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.IO;

namespace Core.Shared.Services.Background.Vision;

public class MatchService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<PurgeService> _logger;
    private readonly IConfiguration _configuration;

    public MatchService(
        ILogger<PurgeService> logger,
        IServiceScopeFactory factory,
        IConfiguration configuration)
    {
        _logger = logger;
        _factory = factory;
        _configuration = configuration;
    }

    const string sig_static_params_file = "C:\\d\\ADSVision\\DLLVision\\dll\\ConfigStatic\\sign_params_static_01.xml";
    const string sig_static_params_file1 = "C:\\d\\ADSVision\\DLLVision\\params\\signature_static.xml";

	const string sig_dynamic_params_file
		= "C:\\d\\ADSVision\\apialarms\\ApiSign\\dll\\ConfigDynamic\\signature_dynamic_CT.xml";

    const string match_dynamic_params_file = "C:\\d\\ADSVision\\DLLVision\\dll\\ConfigDynamic\\match_params_dyn_00.xml";

    const string match_image_folder = "C:\\d\\ADSVision\\ToMatch";

	const string sign_folder = "C:\\d\\ADSVision\\sign";

	const int dataset = 0;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

        int signTimer = _configuration.GetValueWithThrow<int>("SignTimer");
        using PeriodicTimer timer = new (TimeSpan.FromSeconds(signTimer));

        int retInit = DLLVisionImport.fcx_init();
        int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, sig_static_params_file);

        int signParamsDynOutput = DLLVisionImport.fcx_register_sign_params_dynamic(0, sig_dynamic_params_file);

        int matchParamsDynOutput = DLLVisionImport.fcx_register_match_params_dynamic(0, match_dynamic_params_file);
        int datasetRegister = DLLVisionImport.fcx_register_dataset(dataset, 0, 0);

        while (await timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            try
			{
				//Filling load loop of dataset 0
				foreach (string signature_file in Directory.GetFiles(sign_folder, "*.san"))
				{
					string filename = new FileInfo(signature_file).Name;
					string anodeId = Path.GetFileNameWithoutExtension(filename);
					int loadAnodeCode = DLLVisionImport.fcx_load_anode(0, sign_folder, anodeId);
				}

				int returnSigner = DLLVisionImport.fcx_sign(0, 0, match_image_folder, "toto", "temp");
				IntPtr matchRet = DLLVisionImport.fcx_match(0, 0, "temp", "toto");
				int matchErrorCode = DLLVisionImport.fcx_matchRet_errorCode(matchRet);

				int similarityScore = DLLVisionImport.fcx_matchRet_similarityScore(matchRet);
				Console.WriteLine("Similarité : " + similarityScore.ToString());
				string anodeIdFound = Marshal.PtrToStringAnsi(DLLVisionImport.fcx_matchRet_anodeId(matchRet));
				int worstScore = DLLVisionImport.fcx_matchRet_worstScore(matchRet);
				int nbBests = DLLVisionImport.fcx_matchRet_nbBests(matchRet);
				//int bestScore = DLLVisionImport.fcx_matchRet_bestScore(matchRet);
				double mean = DLLVisionImport.fcx_matchRet_mean(matchRet);
				double variance = DLLVisionImport.fcx_matchRet_variance(matchRet);
				long elasped = DLLVisionImport.fcx_matchRet_elapsed(matchRet);
				int threshold = DLLVisionImport.fcx_matchRet_threshold(matchRet);
				int cardinality = DLLVisionImport.fcx_matchRet_cardinality_after_brut_force(matchRet);
				_logger.LogInformation("Return code de la signature: " + matchErrorCode);

				Console.WriteLine("\nerrCode = "
					+ matchErrorCode
					+ ", query = toto, found = "
					+ anodeIdFound
					+ ", similarityScore = "
					+ similarityScore
					+ ", worstScore = "
					+ worstScore
					+ ", bestScore = " + //bestScore +
					", nbBests = "
					+ nbBests
					+ ", mean = "
					+ mean
					+ ", variance = "
					+ variance
					+ ", elapsed = "
					+ elasped
					+ ", threshold = "
					+ threshold
					+ ", cardinality = "
					+ cardinality
					+ "\n");
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
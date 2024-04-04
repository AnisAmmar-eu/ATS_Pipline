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

public class SignFileSettingService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<SignFileSettingService> _logger;
    private readonly IConfiguration _configuration;

	public SignFileSettingService(
		ILogger<SignFileSettingService> logger,
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

		int FileSettingsTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.FileSettingTimer);
		string signStaticParamsFile = _configuration.GetValueWithThrow<string>(ConfigDictionary.SignStaticParams);
        string signDynamicParamsFile = _configuration.GetValueWithThrow<string>(ConfigDictionary.SignDynParams);
		string archivePath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ArchivePath);
		using PeriodicTimer timer = new (TimeSpan.FromSeconds(FileSettingsTimer));

		/// <summary>Logic SignFileSetting</summary>
		/// <param name="signStaticParamsFile">Path to the file with static parameters</param>
		/// <param name="signDynamicParamsFile">Path to the file with dynamic parameters</param>
		/// <remarks>Reads the file with static parameters and dynamic parameters</remarks>
		/// <remarks>If exists sets them in the DLL then move them in archive</remarks>
        while (await timer.WaitForNextTickAsync(stoppingToken)
               && !stoppingToken.IsCancellationRequested)
        {
			try
			{
				int responseStatic = 1000;
				if (File.Exists(signStaticParamsFile)
					&& ((responseStatic = DLLVisionImport.fcx_unregister_sign_params_static(0)) == 0)
					&& ((responseStatic = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParamsFile)) == 0))
				{
					File.Delete(Path.Combine(archivePath, Path.GetFileName(signStaticParamsFile)));
                    File.Move(signStaticParamsFile, Path.Combine(archivePath, Path.GetFileName(signStaticParamsFile)));
				}

				int responseDynamic = 1000;
				if (File.Exists(signDynamicParamsFile)
					&& ((responseDynamic = DLLVisionImport.fcx_unregister_sign_params_dynamic(0)) == 0)
					&& ((responseDynamic = DLLVisionImport.fcx_register_sign_params_dynamic(
						0,
						signDynamicParamsFile)) == 0))
				{
					File.Delete(Path.Combine(archivePath, Path.GetFileName(signDynamicParamsFile)));
					File.Move(signDynamicParamsFile, Path.Combine(archivePath, Path.GetFileName(signDynamicParamsFile)));
				}

				if (responseStatic != 0 || responseDynamic != 0)
				{
					_logger.LogError("Failed to execute FileSettingService with responseStatic"
						+ $"{responseStatic.ToString()} and responseDynamic {responseDynamic.ToString()}.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute FileSettingService with exception message {message}.",
					ex.Message);
			}
		}
	}
}
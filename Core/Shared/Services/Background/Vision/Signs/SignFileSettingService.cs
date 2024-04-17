using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DLLVision;

namespace Core.Shared.Services.Background.Vision.Signs;

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
		int FileSettingsTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.FileSettingTimer);
		string archivePath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ArchivePath);
		string folderParams = _configuration.GetValueWithThrow<string>(ConfigDictionary.FolderParams);
		string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
		string stationName = _configuration.GetValueWithThrow<string>(ConfigDictionary.StationName);

		string folderWithoutCam = Path.Combine(folderParams, stationName, anodeType);

		/// <summary>Logic SignFileSetting</summary>
		/// <param name="signStaticParamsFile">Path to the file with static parameters</param>
		/// <param name="signDynamicParamsFile">Path to the file with dynamic parameters</param>
		/// <remarks>Reads the file with static parameters and dynamic parameters</remarks>
		/// <remarks>If exists sets them in the DLL then move them in archive</remarks>
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(FileSettingsTimer), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

			try
			{
				string signStaticParamsFile = Path.Combine(folderWithoutCam, ConfigDictionary.StaticSignName);
				int responseStatic = 1000;
				if (File.Exists(signStaticParamsFile)
					&& (responseStatic = DLLVisionImport.fcx_unregister_sign_params_static(0)) == 0
					&& (responseStatic = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParamsFile)) == 0)
				{
					File.Delete(Path.Combine(archivePath, Path.GetFileName(signStaticParamsFile)));
					File.Move(signStaticParamsFile, Path.Combine(archivePath, Path.GetFileName(signStaticParamsFile)));
				}

				foreach (int cameraID in new int[] { 1, 2 })
				{
					string signDynamicParamsFile = Path.Combine(
						folderWithoutCam,
						cameraID.ToString(),
						ConfigDictionary.DynamicSignName);

					int responseDynamic = 1000;
					if (File.Exists(signDynamicParamsFile)
						&& (responseDynamic = DLLVisionImport.fcx_unregister_sign_params_dynamic(cameraID)) == 0
						&& (responseDynamic = DLLVisionImport.fcx_register_sign_params_dynamic(
							cameraID,
							signDynamicParamsFile)) == 0)
					{
						File.Delete(Path.Combine(archivePath, Path.GetFileName(signDynamicParamsFile)));
						File.Move(signDynamicParamsFile, Path.Combine(archivePath, Path.GetFileName(signDynamicParamsFile)));
					}

					_logger.LogInformation(
						"Sign FileSettingService with responseStatic {static} and responseDynamic {id} {dynamic}.",
						responseStatic,
						cameraID,
						responseDynamic);
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
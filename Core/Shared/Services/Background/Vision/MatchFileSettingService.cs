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

public class MatchFileSettingService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<MatchFileSettingService> _logger;
	private readonly IConfiguration _configuration;

	public MatchFileSettingService(
		ILogger<MatchFileSettingService> logger,
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
		string matchDynamicParamsFile = _configuration.GetValueWithThrow<string>(ConfigDictionary.MatchDynParams);
		string archivePath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ArchivePath);
		using PeriodicTimer timer = new (TimeSpan.FromSeconds(FileSettingsTimer));

		/// <summary>Logic SignFileSetting</summary>
		/// <param name="matchDynamicParamsFile">Path to the file with dynamic parameters</param>
		/// <remarks>Reads the file with static parameters and dynamic parameters</remarks>
		/// <remarks>If exists sets them in the DLL then move them in archive</remarks>
		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
		{
			try
			{
				int responseDynamic = 1000;
				if (File.Exists(matchDynamicParamsFile)
					&& ((responseDynamic = DLLVisionImport.fcx_unregister_match_params_dynamic(0)) == 0)
					&& ((responseDynamic = DLLVisionImport.fcx_register_match_params_dynamic(0, matchDynamicParamsFile)) == 0))
				{
					File.Move(matchDynamicParamsFile, Path.Combine(archivePath, Path.GetFileName(matchDynamicParamsFile)));
				}

				if (responseDynamic != 0)
					_logger.LogError($"Failed to execute FileSettingService with responseDynamic {responseDynamic.ToString()}.");
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
using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Shared.Services.Background
{
	public class ModeDebugService : BackgroundService
	{
		private readonly IServiceScopeFactory _factory;
		private readonly ILogger<ModeDebugService> _logger;
		private readonly IConfiguration _configuration;

		public ModeDebugService(ILogger<ModeDebugService> logger, IServiceScopeFactory factory, IConfiguration configuration)
		{
			_logger = logger;
			_factory = factory;
			_configuration = configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

				try
				{
					List<DebugMode> settings = await anodeUOW.DebugMode.GetAll();

					if (settings is not null)
					{
						foreach (DebugMode debugMode in settings)
						{
							await ApplyDebugModeAsync(debugMode);
							await ApplyLogAsync(debugMode);
							await ApplyCsvExportAsync(debugMode);
						}
					}

					await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError("Failed to execute ModeDebugService: {Message}", ex.Message);
				}
			}
		}

		private async Task ApplyDebugModeAsync(DebugMode debugMode)
		{
			int result;
			if (debugMode.DebugModeEnabled)
			{
				string debugPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.DebugModePath);

				result = DLLVisionImport.fcx_activate_pipeline_debug(debugPath);
				if (result != 0)
				{
					_logger.LogError("Erreur lors de l'activation du mode de débogage : {ErrorCode}", result);
					return;
				}

				_logger.LogInformation("Debug mode activé avec le chemin : {Path}", debugPath);
			}
			else
			{
				result = DLLVisionImport.fcx_ats_deactivate_pipeline_debug();
				if (result != 0)
				{
					_logger.LogError("Erreur lors de la désactivation du mode de débogage : {ErrorCode}", result);
					return;
				}

				_logger.LogInformation("Debug mode désactivé");
			}
		}

		private async Task ApplyLogAsync(DebugMode debugMode)
		{
			string logPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.LogPath);
			int resultLevel;
			int resultType;
			if (debugMode.LogEnabled)
			{
				resultLevel = DLLVisionImport.fcx_ats_set_log_level(debugMode.LogSeverity);
				resultType = DLLVisionImport.fcx_ats_set_log_type(logPath);

				if (resultLevel != 0)
				{
					_logger.LogError("Erreur lors du réglage du niveau de journalisation : {ErrorCode}", resultLevel);
					return;
				}

				if (resultType != 0)
				{
					_logger.LogError("Erreur lors du réglage du type de journalisation : {ErrorCode}", resultType);
					return;
				}

				_logger.LogInformation(
					"Journalisation activée avec la gravité : {Severity} et le chemin : {Path}",
					debugMode.LogSeverity,
					logPath);
			}
			else
			{
				resultLevel = DLLVisionImport.fcx_ats_set_log_level("off");
				if (resultLevel != 0)
				{
					_logger.LogError("Erreur lors de la désactivation de la journalisation : {ErrorCode}", resultLevel);
					return;
				}

				_logger.LogInformation("Journalisation désactivée");
			}
		}

		private async Task ApplyCsvExportAsync(DebugMode debugMode)
		{
			string csvPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.CsvExportPath);
			int result;
			if (debugMode.CsvExportEnabled)
			{
				result = DLLVisionImport.fcx_ats_activate_score_buffering(csvPath);
				if (result != 0)
				{
					_logger.LogError("Erreur lors de l'activation de l'export CSV : {ErrorCode}", result);
					return;
				}

				_logger.LogInformation("Export CSV activé avec le chemin : {Path}", csvPath);
			}
			else
			{
				result = DLLVisionImport.fcx_ats_deactivate_score_buffering();
				if (result != 0)
				{
					_logger.LogError("Erreur lors de la désactivation de l'export CSV : {ErrorCode}", result);
					return;
				}

				_logger.LogInformation("Export CSV désactivé");
			}
		}
	}
}

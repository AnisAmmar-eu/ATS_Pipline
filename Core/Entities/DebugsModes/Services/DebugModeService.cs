using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Entities.DebugsModes.Repositories;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Entities.DebugsModes.Services;

public class DebugModeService : BaseEntityService<IDebugModeRepository, DebugMode, DTODebugMode>, IDebugModeService
{
	private readonly ILogger<DebugModeService> _logger;
	private readonly IConfiguration _configuration;
	private readonly IDebugModeRepository _repository;

	public DebugModeService(
		IAnodeUOW anodeUOW,
		ILogger<DebugModeService> logger,
		IConfiguration configuration,
		IDebugModeRepository repository) :
		base(anodeUOW)
	{
		_logger = logger;
		_configuration = configuration;
		_repository = repository;
	}

	public async Task<bool> ApplyDebugMode(bool enabled)
	{
		int result;
		if (enabled)
		{
			string debugPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.DebugModePath);

			result = DLLVisionImport.fcx_activate_pipeline_debug(debugPath);
			if (result != 0)
				_logger.LogError("Erreur lors de l'activation du mode de débogage : {ErrorCode}", result);

			_logger.LogInformation("Debug mode activé avec le chemin : {Path}", debugPath);
		}
		else
		{
			result = DLLVisionImport.fcx_ats_deactivate_pipeline_debug();
			if (result != 0)
				_logger.LogError("Erreur lors de la désactivation du mode de débogage : {ErrorCode}", result);

			_logger.LogInformation("Debug mode désactivé");
		}

		List<DebugMode> debugModes = await _repository.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.DebugModeEnabled = enabled;
			_repository.Update(debugMode);
		}

		return true;
	}

	public async Task<bool> ApplyLog(bool enabled, string severity)
	{
		string logPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.LogPath);
		int resultLevel;
		int resultType;
		if (enabled)
		{
			resultLevel = DLLVisionImport.fcx_ats_set_log_level(severity);
			resultType = DLLVisionImport.fcx_ats_set_log_type(logPath);

			if (resultLevel != 0)
				_logger.LogError("Erreur lors du réglage du niveau de journalisation : {ErrorCode}", resultLevel);

			if (resultType != 0)
				_logger.LogError("Erreur lors du réglage du type de journalisation : {ErrorCode}", resultType);

			_logger.LogInformation(
				"Journalisation activée avec la gravité : {Severity} et le chemin : {Path}",
				severity,
				logPath);
		}
		else
		{
			resultLevel = DLLVisionImport.fcx_ats_set_log_level("off");
			if (resultLevel != 0)
				_logger.LogError("Erreur lors de la désactivation de la journalisation : {ErrorCode}", resultLevel);

			_logger.LogInformation("Journalisation désactivée");
		}
		List<DebugMode> debugModes = await _repository.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.LogEnabled = enabled;
			debugMode.LogSeverity = severity;
			_repository.Update(debugMode);
		}

		return true;
	}

	public async Task<bool> ApplyCsvExport(bool enabled)
	{
		string csvPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.CsvExportPath);
		int result;
		if (enabled)
		{
			result = DLLVisionImport.fcx_ats_activate_score_buffering(csvPath);
			if (result != 0)
				_logger.LogError("Erreur lors de l'activation de l'export CSV : {ErrorCode}", result);

			_logger.LogInformation("Export CSV activé avec le chemin : {Path}", csvPath);
		}
		else
		{
			result = DLLVisionImport.fcx_ats_deactivate_score_buffering();
			if (result != 0)
				_logger.LogError("Erreur lors de la désactivation de l'export CSV : {ErrorCode}", result);

			_logger.LogInformation("Export CSV désactivé");
		}

		List<DebugMode> debugModes = await _repository.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.CsvExportEnabled = enabled;
			_repository.Update(debugMode);
		}

		return true;
	}
}
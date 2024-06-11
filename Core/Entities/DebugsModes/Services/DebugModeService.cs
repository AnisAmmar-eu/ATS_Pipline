using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Entities.DebugsModes.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Entities.DebugsModes.Services;

public class DebugModeService : BaseEntityService<IDebugModeRepository, DebugMode, DTODebugMode>, IDebugModeService
{
	private readonly ILogger<DebugModeService> _logger;

	public DebugModeService(
		IAnodeUOW anodeUOW,
		ILogger<DebugModeService> logger) :
		base(anodeUOW)
	{
		_logger = logger;
	}

	public async Task<bool> ApplyDebugMode(bool enabled)
	{
		List<DebugMode> debugModes = await _anodeUOW.DebugMode.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.DebugModeEnabled = enabled;
			_anodeUOW.DebugMode.Update(debugMode);
			_anodeUOW.Commit();
		}

		_logger.LogInformation("Debug mode activé");
		return true;
	}

	public async Task<bool> ApplyLog(bool enabled)
	{
		List<DebugMode> debugModes = await _anodeUOW.DebugMode.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.LogEnabled = enabled;
			_anodeUOW.DebugMode.Update(debugMode);
			_anodeUOW.Commit();
		}

		_logger.LogInformation("Log mode activé");
		return true;
	}

	public async Task<bool> SetSeverity(string severity)
	{
		List<DebugMode> debugModes = await _anodeUOW.DebugMode.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.LogSeverity = severity;
			_anodeUOW.DebugMode.Update(debugMode);
			_anodeUOW.Commit();
		}

		_logger.LogInformation("Journalisation activée avec la gravité : {Severity}", severity);
		return true;
	}

	public async Task<bool> ApplyCsvExport(bool enabled)
	{
		List<DebugMode> debugModes = await _anodeUOW.DebugMode.GetAll();
		if (debugModes is null)
			return false;

		foreach (DebugMode debugMode in debugModes)
		{
			debugMode.CsvExportEnabled = enabled;
			_anodeUOW.DebugMode.Update(debugMode);
			_anodeUOW.Commit();
		}

		_logger.LogInformation("Export CSV activé");
		return true;
	}
}
using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Entities.DebugsModes.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Entities.DebugsModes.Services;

public class DebugModeService : BaseEntityService<IDebugModeRepository, DebugMode, DTODebugMode>, IDebugModeService
{
	private readonly ILogger<DebugModeService> _logger;
	private readonly IConfiguration _configuration;

	public DebugModeService(
		IAnodeUOW anodeUOW,
		ILogger<DebugModeService> logger,
		IConfiguration configuration) :
		base(anodeUOW)
	{
		_logger = logger;
		_configuration = configuration;
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

		_logger.LogInformation("Debug mode set to: {Enabled}", enabled);
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

		_logger.LogInformation("Log set to: {Enabled}", enabled);
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

		_logger.LogInformation("Log severity set to: {Severity}", severity);
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

		_logger.LogInformation("CSV export enabled set to: {Enabled}", enabled);
		return true;
	}
}
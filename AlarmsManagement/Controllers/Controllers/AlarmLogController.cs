using System.ComponentModel.DataAnnotations;
using Core.Entities.AlarmsLog.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlarmsManagement.Controllers.Controllers;

[ApiController]
[Route("api/alarm-log")]
public class AlarmLogController : ControllerBase
{
	private readonly IAlarmLogService _iAlarmLogService;

	public AlarmLogController(IAlarmLogService iAlarmLogService)
	{
		_iAlarmLogService = iAlarmLogService;
	}

	// AlarmPLC controller
	[HttpPost("Collect")]
	public async Task<IActionResult> Collect()
	{
		return Ok(await _iAlarmLogService.Collect());
	}

	// AlarmPLC controller
	[HttpGet("CollectCyc")]
	public async Task<IActionResult> CollectCyc(int nbSecond)
	{
		return Ok(await _iAlarmLogService.CollectCyc(nbSecond));
	}

	/// <summary>
	/// Get all logs.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAllAlarmLog()
	{
		return Ok(await _iAlarmLogService.GetAllAlarmLog());
	}


	/// <summary>
	/// Reads a log entry.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPost("{id}")]
	public async Task<IActionResult> ReadAlarmLog([Required] int id)
	{
		return Ok(await _iAlarmLogService.ReadAlarmLog(id));
	}
}
using System.ComponentModel.DataAnnotations;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Controllers;

[ApiController]
[Route("api/alarm-log")]
public class AlarmLogController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;
	private readonly ILogsService _logsService;

	public AlarmLogController(IAlarmLogService alarmLogService, ILogsService logsService)
	{
		_alarmLogService = alarmLogService;
		_logsService = logsService;
	}

	// AlarmPLC controller
	[HttpPost("Collect")]
	public async Task<IActionResult> Collect()
	{
		try
		{
			await _alarmLogService.Collect();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	// AlarmPLC controller
	[HttpGet("CollectCyc")]
	public async Task<IActionResult> CollectCyc(int nbSecond)
	{
		try
		{
			await _alarmLogService.CollectCyc(nbSecond);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	/// <summary>
	///     Get all logs.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAllAlarmLog()
	{
		List<DTOFAlarmLog> result;
		try
		{
			result = await _alarmLogService.GetAllForFront();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}

	/// <summary>
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route("{alarmClassID}")]
	public async Task<IActionResult> GetAlarmLogByClassID([Required] int alarmClassID)
	{
		List<DTOFAlarmLog> result;
		try
		{
			result = await _alarmLogService.GetByClassID(alarmClassID);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}

	/// <summary>
	///     Ack a list of log entries
	/// </summary>
	/// <param name="alarmLogIDs"></param>
	/// <returns></returns>
	[HttpPost("ack")]
	public async Task<IActionResult> AckAlarmLogs([FromBody] [Required] int[] alarmLogIDs)
	{
		List<DTOFAlarmLog> result;
		try
		{
			result = await _alarmLogService.AckAlarmLogs(alarmLogIDs);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}
}
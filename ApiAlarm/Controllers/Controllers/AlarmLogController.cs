using System.ComponentModel.DataAnnotations;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOF;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Controllers.Controllers;

[ApiController]
[Route("apiAlarm/alarmsLog")]
public class AlarmLogController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;
	private readonly ILogService _logService;

	public AlarmLogController(IAlarmLogService alarmLogService, ILogService logService)
	{
		_alarmLogService = alarmLogService;
		_logService = logService;
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
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logService, ControllerContext);
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
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logService, ControllerContext);
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
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logService, ControllerContext);
	}
}
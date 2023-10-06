using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Controllers;

[ApiController]
[Route("api/alarms-real-time")]
public class AlarmRTController : ControllerBase
{
	private readonly IAlarmRTService _alarmRTService;
	private readonly ILogsService _logsService;

	public AlarmRTController(IAlarmRTService alarmRTService, ILogsService logsService)
	{
		_alarmRTService = alarmRTService;
		_logsService = logsService;
	}

	/// <summary>
	///     Returns all AlarmsRT
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		List<DTOAlarmRT> dtoAlarmRTs;
		try
		{
			dtoAlarmRTs = await _alarmRTService.GetAll();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoAlarmRTs).SuccessResult(_logsService, ControllerContext);
	}

	/// <summary>
	///		This function returns the statistics of AlarmRT. The result will always be an array of length 3.
	/// </summary>
	/// <returns>
	///		res[0] => Nb No Alarms
	///		res[1] => Nb NonAck
	///		res[2] => Nb Active alarms;
	/// </returns>
	[HttpGet("stats")]
	public async Task<IActionResult> GetAlarmRTStats()
	{
		int[] res = new int[3];
		try
		{
			res = await _alarmRTService.GetAlarmRTStats();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(res).SuccessResult(_logsService, ControllerContext);
	}
}
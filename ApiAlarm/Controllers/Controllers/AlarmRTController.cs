using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Controllers.Controllers;

[ApiController]
[Route("apiAlarm/alarmsRealTime")]
public class AlarmRTController : ControllerBase
{
	private readonly IAlarmRTService _alarmRTService;
	private readonly ILogService _logService;

	public AlarmRTController(IAlarmRTService alarmRTService, ILogService logService)
	{
		_alarmRTService = alarmRTService;
		_logService = logService;
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
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(dtoAlarmRTs).SuccessResult(_logService, ControllerContext);
	}

	/// <summary>
	///     This function returns the statistics of AlarmRT. The result will always be an array of length 3.
	/// </summary>
	/// <returns>
	///     res[0] => Nb No Alarms
	///     res[1] => Nb NonAck
	///     res[2] => Nb Active alarms;
	/// </returns>
	[HttpGet("stats")]
	public async Task<IActionResult> GetAlarmRTStats()
	{
		int[] res;
		try
		{
			res = await _alarmRTService.GetAlarmRTStats();
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(res).SuccessResult(_logService, ControllerContext);
	}
}
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
	private readonly ILogsService _logsService;
	private readonly IAlarmRTService _alarmRTService;

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
}
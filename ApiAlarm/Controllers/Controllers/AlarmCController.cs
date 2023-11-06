using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Controllers.Controllers;

[ApiController]
[Route("apiAlarm/alarmsClass")]
public class AlarmCController : ControllerBase
{
	private readonly IAlarmCService _alarmCService;
	private readonly ILogService _logService;

	public AlarmCController(IAlarmCService alarmCService, ILogService logService)
	{
		_alarmCService = alarmCService;
		_logService = logService;
	}

	/// <summary>
	///     Get all alarms classes.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		List<DTOAlarmC> dtoAlarmCs = new();
		try
		{
			dtoAlarmCs = await _alarmCService.GetAll();
		}
		catch (Exception e)
		{
			if (e is EntryPointNotFoundException)
				return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoAlarmCs).SuccessResult(_logService, ControllerContext);
	}
}
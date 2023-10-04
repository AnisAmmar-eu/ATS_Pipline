using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Controllers;

[ApiController]
[Route("api/alarms-class")]
public class AlarmCController : ControllerBase
{
	private readonly ILogsService _logsService;
	private readonly IAlarmCService _alarmCService;

	public AlarmCController(IAlarmCService alarmCService, ILogsService logsService)
	{
		_alarmCService = alarmCService;
		_logsService = logsService;
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
				return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoAlarmCs).SuccessResult(_logsService, ControllerContext);
	}
}
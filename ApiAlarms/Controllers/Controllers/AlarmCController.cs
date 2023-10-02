using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Controllers;

[ApiController]
[Route("api/alarms-class")]
public class AlarmCController : ControllerBase
{
	private readonly IAlarmCService _alarmCService;
	private readonly IAlarmPLCService _alarmPLCServiceA;

	public AlarmCController(IAlarmCService alarmCService, IAlarmPLCService alarmPLCServiceA)
	{
		_alarmCService = alarmCService;
		_alarmPLCServiceA = alarmPLCServiceA;
	}

	[HttpGet("IO")]
	public async Task guifez()
	{
		await _alarmPLCServiceA.Add(new AlarmPLC(new Alarm()));
	}

	/// <summary>
	/// Get all alarms classes.
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
				return new ApiResponseObject(e.Message).BadRequestResult();
		}

		return new ApiResponseObject(dtoAlarmCs).SuccessResult();
	}
}
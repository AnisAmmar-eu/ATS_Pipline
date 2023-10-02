using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Controllers;

[ApiController]
[Route("api/alarms-real-time")]
public class AlarmRTController : ControllerBase
{
	private readonly IAlarmRTService _alarmRTService;

	public AlarmRTController(IAlarmRTService alarmRTService)
	{
		_alarmRTService = alarmRTService;
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
			if (e is EntryPointNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();
			Console.WriteLine("Internal error: " + e.Message);
			return StatusCode(500, e.Message);
		}

		return new ApiResponseObject(dtoAlarmRTs).SuccessResult();
	}
}
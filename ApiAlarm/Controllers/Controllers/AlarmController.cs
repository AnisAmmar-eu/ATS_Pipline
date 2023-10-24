using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Controllers.Controllers;

[ApiController]
[Route("apiAlarm")]
public class AlarmController : ControllerBase
{
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
	}
}
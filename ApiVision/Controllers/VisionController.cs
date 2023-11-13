using Core.Entities.StationCycles.Services;
using Core.Shared.Attributes;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiVision.Controllers;

[ApiController]
[Route("apiVision")]
[ServerAction]
public class VisionController : ControllerBase
{
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
	}
}
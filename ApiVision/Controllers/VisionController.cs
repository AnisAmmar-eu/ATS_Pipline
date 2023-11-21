using Core.Shared.Attributes;
using Core.Shared.Models.HttpResponse;
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
		return new ControllerResponseObject().SuccessResult();
	}
}
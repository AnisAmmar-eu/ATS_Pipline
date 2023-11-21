using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

[ApiController]
[Route("apiIOT")]
public class IOTController : ControllerBase
{
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ControllerResponseObject().SuccessResult();
	}
}
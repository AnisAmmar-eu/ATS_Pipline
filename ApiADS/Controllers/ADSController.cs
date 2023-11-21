using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiADS.Controllers;

[ApiController]
[Route("apiADS")]
public class ADSController : ControllerBase
{
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ControllerResponseObject().SuccessResult();
	}
}
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Core.Shared.Attributes;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiKPI.Controllers;

[ApiController]
[Route("apiKPI")]
[ServerAction]
public class KPIController : ControllerBase
{
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
	}
}
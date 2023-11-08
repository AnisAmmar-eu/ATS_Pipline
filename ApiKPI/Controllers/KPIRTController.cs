using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Core.Shared.Attributes;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiKPI.Controllers;

[ApiController]
[Route("apiKPIRT")]
[ServerAction]
public class KPIRTController : ControllerBase
{
	private readonly IKPIRTService _kpirtService;
	private readonly ILogService _logService;


	public KPIRTController(IKPIRTService kpirtService, ILogService logService)
	{
		_kpirtService = kpirtService;
		_logService = logService;
	}

	[HttpPut]
	[Route("{timePeriod}")]
	public async Task<IActionResult> GetByRIDs([FromRoute] string timePeriod, [FromBody] List<string> rids)
	{
		List<DTOKPIRT> dtos;
		try
		{
			dtos = await _kpirtService.GetByRIDsAndPeriod(timePeriod, rids);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtos).SuccessResult(_logService, ControllerContext);
	}
}
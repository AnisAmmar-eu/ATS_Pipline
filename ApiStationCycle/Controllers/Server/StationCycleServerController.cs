using System.ComponentModel.DataAnnotations;
using Core.Entities.StationCycles.Services;
using Core.Shared.Attributes;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers.Server;

[ApiController]
[Route("apiStationServerCycle")]
[ServerAction]
public class StationCycleServerController : ControllerBase
{
	private readonly ILogService _logService;
	private readonly IStationCycleService _stationCycleService;

	public StationCycleServerController(IStationCycleService stationCycleService, ILogService logService)
	{
		_stationCycleService = stationCycleService;
		_logService = logService;
	}
	
	[HttpGet("signMatchStats")]
	public async Task<IActionResult> GetSignMatchStats()
	{
		int[] stats;
		try
		{
			stats = await _stationCycleService.GetMatchingStats(TimeSpan.FromHours(24));
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}
		
		return await new ApiResponseObject(stats).SuccessResult(_logService, ControllerContext);
	}

	[HttpGet("signMatchStats/{stationID}")]
	public async Task<IActionResult> GetSignMatchStatsByStationID([FromRoute] [Required] int stationID)
	{
		int[] stats;
		try
		{
			stats = await _stationCycleService.GetMatchingStats(TimeSpan.FromHours(24), stationID);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}
		
		return await new ApiResponseObject(stats).SuccessResult(_logService, ControllerContext);
	}
}
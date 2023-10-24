using System.ComponentModel.DataAnnotations;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Services;
using Core.Shared.Exceptions;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers;

[ApiController]
[Route("apiStationCycle")]
public class IOTController : ControllerBase
{
	private readonly ILogsService _logsService;
	private readonly IStationCycleService _stationCycleService;

	public IOTController(IStationCycleService stationCycleService, ILogsService logsService)
	{
		_stationCycleService = stationCycleService;
		_logsService = logsService;
	}

	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
	}

	[HttpGet]
	public async Task<IActionResult> GetAllRIDs()
	{
		List<ReducedStationCycle> result;
		try
		{
			result = await _stationCycleService.GetAllRIDs();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}

	[HttpGet("mostRecent")]
	public async Task<IActionResult> GetMostRecent()
	{
		DTOStationCycle? result;
		try
		{
			result = await _stationCycleService.GetMostRecentWithIncludes();
			if (result == null)
				throw new NoDataException("There is no station cycles yet.");
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetByID([Required] [FromRoute] int id)
	{
		DTOStationCycle result;
		try
		{
			result = await _stationCycleService.GetByID(id);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}

	[HttpGet("{id}/images/{cameraNb}")]
	public async Task<IActionResult> GetImageByIdAndCamera([Required] [FromRoute] int id,
		[Required] [FromRoute] int cameraNb)
	{
		Byte[] result;
		try
		{
			result = await _stationCycleService.GetImagesFromIDAndCamera(id, cameraNb);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return File(result, "image/jpeg");
	}
}
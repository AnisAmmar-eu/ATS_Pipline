using System.ComponentModel.DataAnnotations;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers;

[ApiController]
[Route("apiStationCycle")]
public class StationCycleController : ControllerBase
{
	private readonly ILogService _logService;
	private readonly IStationCycleService _stationCycleService;

	public StationCycleController(IStationCycleService stationCycleService, ILogService logService)
	{
		_stationCycleService = stationCycleService;
		_logService = logService;
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
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logService, ControllerContext);
	}

	[HttpGet("mostRecent")]
	public async Task<IActionResult> GetMostRecent()
	{
		ReducedStationCycle? result;
		try
		{
			result = await _stationCycleService.GetMostRecentWithIncludes();
			if (result == null)
				throw new NoDataException("There is no station cycles yet.");
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logService, ControllerContext);
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
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logService, ControllerContext);
	}

	[HttpGet("{id}/images/{cameraNb}")]
	public async Task<IActionResult> GetImageByIdAndCamera([Required] [FromRoute] int id,
		[Required] [FromRoute] int cameraNb)
	{
		byte[] image;
		DateTimeOffset ts;
		try
		{
			FileInfo imageFile = await _stationCycleService.GetImagesFromIDAndCamera(id, cameraNb);
			ts = imageFile.CreationTime;
			image = await System.IO.File.ReadAllBytesAsync(imageFile.FullName);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
		return File(image, "image/jpeg", ts.ToUnixTimeMilliseconds().ToString());
	}
}
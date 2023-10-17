using System.ComponentModel.DataAnnotations;
using ApiCamera.Utils;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;
using Stemmer.Cvb;

namespace ApiCamera.Controllers;

[ApiController]
[Route("[controller]")]
public class CameraApiController : ControllerBase
{
	private readonly ILogsService _logsService;

	public CameraApiController(ILogsService logsService)
	{
		_logsService = logsService;
	}

	#region Get/Set Device Info

	[HttpGet("/device-info")]
	public IActionResult GetDeviceInfo()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		string result;
		try
		{
			Device device = DeviceFactory.Open(driverString);
			result = device == null ? "Driver Open Problem" : "Driver Opened ";
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok(result);
	}

	#endregion

	#region Parameters

	[HttpPost("/set-parameters")]
	public async Task<ActionResult> SetParameters([FromBody] [Required] Dictionary<string, string> parameters)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		try
		{
			Device device = DeviceFactory.Open(driverString);
			CameraMethod.SetParameters(device, parameters);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	#endregion
	
	#region Acquisition

	[HttpGet("/acquisition")]
	public async Task<IActionResult> AcquisitionAsync()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		try
		{
			// Create an instance of the camera
			Device? device = DeviceFactory.Open(driverString);
			await CameraMethod.RunAcquisitionAsync(device, new DTOCameraParam(), "jpg");
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	#endregion
}
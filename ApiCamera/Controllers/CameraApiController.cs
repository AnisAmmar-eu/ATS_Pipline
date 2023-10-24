using System.ComponentModel.DataAnnotations;
using ApiCamera.Utils;
using Core.Entities.Packets.Dictionaries;
using Core.Shared.Dictionaries;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;
using Stemmer.Cvb;

namespace ApiCamera.Controllers;

[ApiController]
[Route("apiCamera")]
public class CameraApiController : ControllerBase
{
	private readonly IConfiguration _configuration;
	private readonly ILogsService _logsService;

	public CameraApiController(ILogsService logsService, IConfiguration configuration)
	{
		_logsService = logsService;
		_configuration = configuration;
	}
	
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
	}

	#region Acquisition

	[HttpGet("acquisition")]
	public async Task<IActionResult> AcquisitionAsync()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		int port1 = _configuration.GetValue<int>("CameraConfig:Camera1:Port");
		int port2 = _configuration.GetValue<int>("CameraConfig:Camera2:Port");
		try
		{
			Device? device1 = DeviceFactory.OpenPort(driverString, port1);
			if (Station.Type != StationType.S5)
			{
				// Create an instance of the camera
				Device? device2 = DeviceFactory.OpenPort(driverString, port2);
				CameraUtils.RunAcquisition(device1, "jpg", ShootingFolders.Camera1);
				CameraUtils.RunAcquisition(device2, "jpg", ShootingFolders.Camera2);
			}
			else
			{
				CameraUtils.RunAcquisition(device1, "jpg", ShootingFolders.Camera1, ShootingFolders.Camera2);
			}
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	#endregion

	#region Get/Set Device

	[HttpGet("device1")]
	public async Task<IActionResult> GetDevice1Info()
	{
		int port = _configuration.GetValue<int>("CameraConfig:Camera1:Port");
		return await GetDeviceInfo(port);
	}

	[HttpGet("device2")]
	public async Task<IActionResult> GetDevice2Info()
	{
		int port = _configuration.GetValue<int>("CameraConfig:Camera2:Port");
		return await GetDeviceInfo(port);
	}

	[HttpPost("device1")]
	public async Task<ActionResult> SetDevice1Parameters([FromBody] [Required] Dictionary<string, string> parameters)
	{
		int port = _configuration.GetValue<int>("CameraConfig:Camera1:Port");
		return await SetDeviceParameters(port, parameters);
	}

	[HttpPost("device2")]
	public async Task<ActionResult> SetDevice2Parameters([FromBody] [Required] Dictionary<string, string> parameters)
	{
		int port = _configuration.GetValue<int>("CameraConfig:Camera2:Port");
		return await SetDeviceParameters(port, parameters);
	}

	#endregion

	#region Generic functions

	private async Task<IActionResult> GetDeviceInfo(int port)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		string result;
		try
		{
			Device device = DeviceFactory.OpenPort(driverString, port);
			result = device == null ? "Driver Open Problem" : "Driver Opened ";
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}

	private async Task<ActionResult> SetDeviceParameters(int port, Dictionary<string, string> parameters)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		try
		{
			Device device = DeviceFactory.OpenPort(driverString, port);
			CameraUtils.SetParameters(device, parameters);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	#endregion
}
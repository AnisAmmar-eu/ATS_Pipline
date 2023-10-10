using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Entities.Parameters.CameraParams.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;
using TwinCAT.Ads;
using TwinCAT.TypeSystem;

namespace ApiCamera.Controllers;

[ApiController]
[Route("[controller]")]
public class CameraApiController : ControllerBase
{
	private readonly ILogsService _logsService;
	private readonly ICameraParamService _cameraParamService;

	public CameraApiController(ICameraParamService cameraParamService, ILogsService logsService)
	{
		_cameraParamService = cameraParamService;
		_logsService = logsService;
	}

	#region Get/Set Device Info

	[HttpGet("/device-info")]
	public IActionResult GetDeviceInfo()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		string result = "";
		try
		{
			Device device = DeviceFactory.Open(driverString);

			NodeMap deviceNodeMap = device.NodeMaps[NodeMapNames.Device];

			Debug.WriteLine("DeviceTemperature 1: " + deviceNodeMap["DeviceTemperature"] + "°");

			Debug.WriteLine("Vendor " + deviceNodeMap["DeviceVendorName"]);

			Debug.WriteLine("DeviceTLVersionMinor " + deviceNodeMap["DeviceTLVersionMinor"]);

			Debug.WriteLine("DeviceFamilyName " + deviceNodeMap["DeviceFamilyName"]);

			if (deviceNodeMap["ExposureTime"] is FloatNode exposure)
				exposure.Value = 40000.5;

			Debug.WriteLine("DeviceTemperature 2: " + deviceNodeMap["DeviceTemperature"] + "°");


			if (device == null)
				result = "Driver Open Problem";
			else
				result = "Driver Opened ";
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok(result);
	}

	#endregion

	#region Parameters

	[HttpGet("parameters")]
	public async Task<IActionResult> GetDeviceParameters()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		DTOCameraParam dtoCameraParam;
		try
		{
			Device device = DeviceFactory.Open(driverString);
			dtoCameraParam = _cameraParamService.GetDeviceParameters(device);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoCameraParam).SuccessResult(_logsService, ControllerContext);
	}

	[HttpPost("parameters")]
	public async Task<IActionResult> SetDeviceParameters([FromBody] [Required] DTOCameraParam dtoCameraParam)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		try
		{
			Device device = DeviceFactory.Open(driverString);
			_cameraParamService.SetDeviceParameters(device, dtoCameraParam);
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
	public async Task<IActionResult> AcquisitionAsync([Required] string extension)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		try
		{
			// Create an instance of the camera
			Device? device = DeviceFactory.Open(driverString);
			await _cameraParamService.RunAcquisitionAsync(device, new DTOCameraParam(), extension);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);

	}

	#endregion
}
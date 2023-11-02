using ApiCamera.Utils;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.BI.BITemperatures.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Dictionaries;
using Core.Shared.Dictionaries;
using Core.Shared.Models.Camera;
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
	private readonly IIOTTagService _iotTagService;
	private readonly IBITemperatureService _biTemperatureService;

	public CameraApiController(ILogsService logsService, IConfiguration configuration, IIOTTagService iotTagService, IBITemperatureService biTemperatureService)
	{
		_logsService = logsService;
		_configuration = configuration;
		_iotTagService = iotTagService;
		_biTemperatureService = biTemperatureService;
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
		int port1 = _configuration.GetValue<int>("CameraConfig:Camera1:Port");
		int port2 = _configuration.GetValue<int>("CameraConfig:Camera2:Port");
		try
		{
			Device device1 = CameraConnectionManager.Connect(port1);
			if (Station.Type != StationType.S5)
			{
				// Create an instance of the camera
				Device device2 = CameraConnectionManager.Connect(port2);
				Task task1 = CameraUtils.RunAcquisition(_iotTagService, device1, "jpg", ShootingUtils.Camera1,
					ShootingUtils.CameraTest1);
				Task task2 = CameraUtils.RunAcquisition(_iotTagService, device2, "jpg", ShootingUtils.Camera2,
					ShootingUtils.CameraTest2);
				await task1;
				await task2;
			}
			else
			{
				await CameraUtils.RunAcquisition(_iotTagService, device1, "jpg", ShootingUtils.Camera1,
					ShootingUtils.CameraTest1, ShootingUtils.Camera2, ShootingUtils.CameraTest2);
			}
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	#endregion

	#region Get Temperature

	[HttpGet("temperature")]
	public async Task<IActionResult> GetTemperatures()
	{
		List<DTOBITemperature> temperatures;
		try
		{
			temperatures = await _biTemperatureService.GetAll();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(temperatures).SuccessResult(_logsService, ControllerContext);
	}

	#endregion

	#region Get TestImages

	[HttpGet("1/testImage")]
	public async Task<IActionResult> GetCamera1TestImage()
	{
		byte[] image;
		DateTimeOffset ts;
		try
		{
			FileInfo imageFile = new(ShootingUtils.CameraTest1 + ShootingUtils.TestFilename);
			ts = imageFile.CreationTime;
			image = await System.IO.File.ReadAllBytesAsync(imageFile.FullName);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
		return File(image, "image/jpeg", ts.ToString(ADSUtils.TSFormat));
	}

	[HttpGet("2/testImage")]
	public async Task<IActionResult> GetCamera2TestImage()
	{
		byte[] image;
		DateTimeOffset ts;
		try
		{
			FileInfo imageFile = new(ShootingUtils.CameraTest2 + ShootingUtils.TestFilename);
			ts = imageFile.CreationTime;
			image = await System.IO.File.ReadAllBytesAsync(imageFile.FullName);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
		return File(image, "image/jpeg", ts.ToString(ADSUtils.TSFormat));
	}

	#endregion
}
using System.ComponentModel.DataAnnotations;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

[ApiController]
[Route("api/iot-device")]
public class IOTDeviceController : ControllerBase
{
	private IIOTDeviceService _iotDeviceService;
	private ILogsService _logsService;

	public IOTDeviceController(IIOTDeviceService iotDeviceService, ILogsService logsService)
	{
		_iotDeviceService = iotDeviceService;
		_logsService = logsService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllIOTDevices()
	{
		List<DTOIOTDevice> devices;
		try
		{
			devices = await _iotDeviceService.GetAllWithIncludes();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(devices).SuccessResult(_logsService, ControllerContext);
	}

	[HttpGet("{rid}")]
	public async Task<IActionResult> GetIOTDeviceByRID([Required] string rid)
	{
		DTOIOTDevice device;
		try
		{
			device = await _iotDeviceService.GetByRIDWithIncludes(rid);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(device).SuccessResult(_logsService, ControllerContext);
	}
}
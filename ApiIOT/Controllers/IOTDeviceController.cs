using System.ComponentModel.DataAnnotations;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Models.Structs;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

[ApiController]
[Route("apiIOT/iotDevices")]
public class IOTDeviceController : ControllerBase
{
	private readonly IIOTDeviceService _iotDeviceService;
	private readonly ILogService _logService;

	public IOTDeviceController(IIOTDeviceService iotDeviceService, ILogService logService)
	{
		_iotDeviceService = iotDeviceService;
		_logService = logService;
	}

	[HttpGet("status/{rid}")]
	public async Task<IActionResult> GetStatusByRID([Required] [FromRoute] string rid)
	{
		IOTDeviceStatus status;
		try
		{
			status = await _iotDeviceService.GetStatusByRID(rid);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(status).SuccessResult(_logService, ControllerContext);
	}

	[HttpPut("rids")]
	public async Task<IActionResult> GetTagValueByArrayRID([FromBody] [Required] IEnumerable<string> rids)
	{
		List<IOTDeviceStatus> tags;
		try
		{
			tags = await _iotDeviceService.GetStatusByArrayRID(rids);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(tags).SuccessResult(_logService, ControllerContext);
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
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(devices).SuccessResult(_logService, ControllerContext);
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
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(device).SuccessResult(_logService, ControllerContext);
	}
}
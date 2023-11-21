using System.Linq.Expressions;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiCameraAssign.Controllers;

[ApiController]
[Route("apiCameraAssign")]
public class CameraAssignController : ControllerBase
{
	private readonly ILogService _logService;
	private readonly IPacketService _packetService;

	public CameraAssignController(ILogService logService, IPacketService packetService)
	{
		_logService = logService;
		_packetService = packetService;
	}

	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ControllerResponseObject().SuccessResult();
	}

	[HttpGet]
	public async Task<IActionResult> GetAllShootings()
	{
		List<DTOShooting> packets;
		try
		{
			packets = (await _packetService.GetAll(new Expression<Func<Packet, bool>>[]
			{
				packet => packet is Shooting
			})).ConvertAll(packet => packet as DTOShooting)!;
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(packets).SuccessResult(_logService, ControllerContext);
	}

	[HttpPost]
	public async Task<IActionResult> BuildShooting()
	{
		DTOPacket packet;
		try
		{
			packet = await _packetService.BuildPacket(new Shooting());
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject(packet).SuccessResult(_logService, ControllerContext);
	}
}
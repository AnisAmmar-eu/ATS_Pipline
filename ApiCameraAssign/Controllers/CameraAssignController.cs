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
	private readonly ILogsService _logsService;
	private readonly IPacketService _packetService;

	public CameraAssignController(ILogsService logsService, IPacketService packetService)
	{
		_logsService = logsService;
		_packetService = packetService;
	}

	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
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
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(packets).SuccessResult(_logsService, ControllerContext);
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
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(packet).SuccessResult(_logsService, ControllerContext);
	}
}
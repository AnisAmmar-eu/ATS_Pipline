using System.Linq.Expressions;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiCameraAssign.Controllers;

[ApiController]
[Route("[controller]")]
public class CameraAssignController : ControllerBase
{
	private readonly ILogsService _logsService;
	private readonly IPacketService _packetService;

	public CameraAssignController(ILogsService logsService, IPacketService packetService)
	{
		_logsService = logsService;
		_packetService = packetService;
	}

	[HttpPost("announce")]
	public async Task<IActionResult> BuildAnnounce([FromQuery] string rid)
	{
		DTOAnnouncement? announcement;
		try
		{
			Announcement ann = new()
			{
				Type = PacketType.Announcement,
				StationCycleRID = rid,
				AnodeType = AnodeTypeDict.DX
			};
			announcement = await _packetService.BuildPacket(ann) as DTOAnnouncement;
			if (announcement == null)
				throw new Exception("Cast error");
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(announcement).SuccessResult(_logsService, ControllerContext);
	}

	[HttpPost("detection")]
	public async Task<IActionResult> BuildDetection([FromQuery] string rid)
	{
		DTODetection? announcement;
		try
		{
			Detection ann = new()
			{
				Type = PacketType.Detection,
				StationCycleRID = rid,
				AnodeSize = 42
			};
			announcement = await _packetService.BuildPacket(ann) as DTODetection;
			if (announcement == null)
				throw new Exception("Cast error");
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(announcement).SuccessResult(_logsService, ControllerContext);
	}

	[HttpGet]
	public async Task<IActionResult> GetAllShootings()
	{
		List<DTOShooting> packets;
		try
		{
			packets = (await _packetService.GetAll(new Expression<Func<Packet, bool>>[]
			{
				packet => packet.Type == PacketType.Shooting
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
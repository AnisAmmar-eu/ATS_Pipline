using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiPacket.Controllers;

[ApiController]
[Route("api/packets")]
public class PacketController : ControllerBase
{
	private readonly ILogsService _logsService;
	private readonly IPacketService _packetService;

	public PacketController(IPacketService packetService, ILogsService logsService)
	{
		_packetService = packetService;
		_logsService = logsService;
	}

	[HttpGet("AAAAAAA")]
	public async Task<IActionResult> BuildFurnace()
	{
		OutFurnace furnace = new()
		{
			OutAnnounceID = "salut",
			FTAPickUp = 42,
			PickUpTS = DateTimeOffset.Now,
			DepositTS = DateTimeOffset.Now,
			InvalidPacket = 42,
			StationCycleRID = "Feur",
			Status = PacketStatus.Completed
		};
		await _packetService.BuildPacket(furnace);
		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	[HttpPost("alarms")]
	public async Task<IActionResult> BuildAlarmPacket([FromBody] DTOAlarmList dtoAlarmList)
	{
		DTOPacket result;
		try
		{
			result = await _packetService.BuildPacket(dtoAlarmList.ToModel());
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(result).SuccessResult(_logsService, ControllerContext);
	}
}
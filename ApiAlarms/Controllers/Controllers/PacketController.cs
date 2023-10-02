using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Services;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Controllers;

[ApiController]
[Route("api/packets")]
public class PacketController : ControllerBase
{
	private readonly IPacketService _packetService;

	public PacketController(IPacketService packetService)
	{
		_packetService = packetService;
	}

	[HttpPost("alarms")]
	public async Task<IActionResult> BuildAlarmPacket([FromBody] DTOAlarmList dtoAlarmList)
	{
		return new ApiResponseObject(await _packetService.BuildPacket(dtoAlarmList)).SuccessResult();
	}
}
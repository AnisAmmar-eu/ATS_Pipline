using Core.Entities.Packets.Models.DTO.AlarmListPackets;
using Core.Entities.Packets.Services;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiPacket.Controllers;

[ApiController]
[Route("api/packets")]
public class PacketController : ControllerBase
{
	
	private IPacketService _packetService;

	public PacketController(IPacketService packetService)
	{
		_packetService = packetService;
	}

	[HttpPost("alarms")]
	public async Task<IActionResult> BuildAlarmPacket([FromBody] DTOAlarmListPacket dtoAlarmListPacket)
	{
		return new ApiResponseObject(await _packetService.BuildPacket(dtoAlarmListPacket)).SuccessResult();
	}
}
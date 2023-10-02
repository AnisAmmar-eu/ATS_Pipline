using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers.Transfer;

[ApiController]
[Route("api/transfer")]
public class TransferController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;
	private readonly IPacketService _packetService;

	public TransferController(IAlarmLogService alarmLogService, IPacketService packetService)
	{
		_alarmLogService = alarmLogService;
		_packetService = packetService;
	}


	[HttpPost("alarm-log")]
	public async Task<IActionResult> TransferAlarmsLog()
	{
		try
		{
			HttpResponseMessage response = await _alarmLogService.SendLogsToServer();
			if (response.IsSuccessStatusCode)
				return Ok(true);
			string errorMessage = await response.Content.ReadAsStringAsync();
			return StatusCode((int)response.StatusCode, errorMessage);
		}
		catch (Exception ex)
		{
			// Handle any exception
			Console.WriteLine($"Une erreur s'est produite lors de l'envoi de la requête : {ex.Message}");
			return StatusCode(500, "Erreur interne du serveur");
		}
	}

	[HttpPost("packet")]
	public async Task<IActionResult> TransferPackets()
	{
		try
		{
			HttpResponseMessage response = await _packetService.SendPacketsToServer();
			if (response.IsSuccessStatusCode)
				return Ok(true);
			string errorMessage = await response.Content.ReadAsStringAsync();
			return StatusCode((int)response.StatusCode, errorMessage);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Une erreur s'est produite lors de l'envoi de la requête : {ex.Message}");
			return StatusCode(500, "Erreur interne du serveur");
		}
	}
}
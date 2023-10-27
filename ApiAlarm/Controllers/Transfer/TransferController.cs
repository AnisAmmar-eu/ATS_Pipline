using Core.Entities.Alarms.AlarmsLog.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Controllers.Transfer;

[ApiController]
[Route("apiAlarm/transfer")]
public class TransferController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;

	public TransferController(IAlarmLogService alarmLogService)
	{
		_alarmLogService = alarmLogService;
	}


	[HttpPost("alarmsLog")]
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
}
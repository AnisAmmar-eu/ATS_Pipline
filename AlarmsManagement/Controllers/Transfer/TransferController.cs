using System.Text;
using Core.Entities.AlarmsC.Services;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsLog.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlarmsManagement.Controllers.Transfer;

[ApiController]
[Route("api/transfer")]
public class TransferController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;

	public TransferController(IAlarmLogService alarmLogService, IAlarmCService alarmCService)
	{
		_alarmLogService = alarmLogService;
	}


	[HttpPost("alarm-log")]
	public async Task<IActionResult> TransferAlarmLog()
	{
		try
		{ 
			HttpResponseMessage response = await _alarmLogService.SendLogsToServer();
			if (response.IsSuccessStatusCode)
				return Ok(true);
			var errorMessage = await response.Content.ReadAsStringAsync();
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
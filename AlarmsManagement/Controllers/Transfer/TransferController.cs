using System.Text;
using Core.Entities.AlarmsC.Services;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsLog.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlarmsManagement.Controllers.Transfer;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;

	public TransferController(IAlarmLogService alarmLogService, IAlarmCService alarmCService)
	{
		_alarmLogService = alarmLogService;
	}


	[HttpPost("PushDataToApi2Async")]
	public async Task<IActionResult> PushDataToApi2Async()
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
			// Gérer les erreurs d'exception
			Console.WriteLine($"Une erreur s'est produite lors de l'envoi de la requête : {ex.Message}");
			return StatusCode(500, "Erreur interne du serveur");
		}
	}
}
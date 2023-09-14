using System.Text;
using Core.Entities.AlarmsC.Services;
using Core.Entities.AlarmsLog.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlarmsManagement.Controllers.Transfer;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
	private readonly HttpClient _httpClient;
	private readonly IAlarmLogService _alarmLogService;
	private readonly IAlarmCService _alarmCService;

	public TransferController(IAlarmLogService alarmLogService, IAlarmCService alarmCService)
	{
		_httpClient = new HttpClient();
		_alarmLogService = alarmLogService;
		_alarmCService = alarmCService;
	}


	[HttpPost("PushDataToApi2Async")]
	public async Task<IActionResult> PushDataToApi2Async()
	{
		try
		{
			var api2Url = "https://localhost:7207/api/Receive/endpoint";

			var journals = await _alarmLogService.GetAllAlarmLog();
			foreach (var dtoJournal in journals)
			{
				dtoJournal.Alarm = await _alarmCService.GetById(dtoJournal.AlarmID);
			}

			var jsonData = JsonConvert.SerializeObject(journals);

			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsync(api2Url, content);

				if (response.IsSuccessStatusCode)
				{
					return Ok(true); // Les données ont été envoyées avec succès à l'API 2
				}

				// Gérer le cas où la requête a échoué
				var errorMessage = await response.Content.ReadAsStringAsync();
				return StatusCode((int)response.StatusCode, errorMessage);
			}
		}
		catch (Exception ex)
		{
			// Gérer les erreurs d'exception
			Console.WriteLine($"Une erreur s'est produite lors de l'envoi de la requête : {ex.Message}");
			return StatusCode(500, "Erreur interne du serveur");
		}
	}
}
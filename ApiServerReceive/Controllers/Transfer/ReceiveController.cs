using System.Diagnostics;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsC.Services;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Entities.Journals.Services;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServerReceive.Controllers.Transfer;

[Route("api/[controller]")]
[ApiController]
public class ReceiveController : ControllerBase
{
	private readonly AlarmCTX _alarmCtx;


	private readonly HttpClient _httpClient;
	private readonly IJournalService _iJournalService;
	private readonly IAlarmCService _iAlarmCService;

	public ReceiveController(IJournalService journalService, AlarmCTX alarmCTX, IAlarmCService iAlarmCService)
	{
		_httpClient = new HttpClient();
		_iJournalService = journalService;
		_alarmCtx = alarmCTX;
		_iAlarmCService = iAlarmCService;
	}


	[HttpPost]
	[Route("endpoint")]
	public async Task<IActionResult> ReceiveDataFromApi1([FromBody] IEnumerable<DTOJournal> dtoJournals)
	{
		try
		{
			Debug.Print("Reçu depuis l'api 1");

			if (dtoJournals == null || !dtoJournals.Any()) return BadRequest("Aucun journal à traiter.");
			var truncateSql = "TRUNCATE TABLE Journal ";
			var truncateSqlRT = "TRUNCATE TABLE AlarmRT ";
			_alarmCtx.Database.ExecuteSqlRaw(truncateSql);
			_alarmCtx.Database.ExecuteSqlRaw(truncateSqlRT);


			foreach (var journal in dtoJournals)
			{
				if (journal.Alarm != null)
				{
					DTOAlarmC newAlarmC = await _iAlarmCService.AddReceivedAlarmC(journal.Alarm);
					journal.Alarm = newAlarmC;
					journal.IDAlarm = newAlarmC.ID;
				}
				else throw new EntityNotFoundException("There is no AlarmC in the transmitted journal");

				var journalToAdd = new Journal
				{
					IDAlarm = journal.IDAlarm,
					Status1 = journal.Status1,
					TS1 = journal.TS1,
					Status0 = journal.Status0,
					TS0 = journal.TS0,
					IsRead = 0,
					Station = journal.Station,
					TSRead = null
				};
				await _iJournalService.AddJournal(journalToAdd);

				// _IJournalServices.AddJournalFromPush(journalToAdd);
			}

			return Ok(true);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return StatusCode(500, "Erreur interne du serveur");
		}
	}
}
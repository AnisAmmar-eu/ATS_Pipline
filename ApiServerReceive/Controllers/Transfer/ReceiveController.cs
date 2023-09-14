using System.Diagnostics;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsC.Services;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsLog.Services;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace ApiServerReceive.Controllers.Transfer;

[Route("api/[controller]")]
[ApiController]
public class ReceiveController : ControllerBase
{
	private readonly AlarmCTX _alarmCtx;


	private readonly HttpClient _httpClient;
	private readonly IAlarmLogService _iAlarmLogService;
	private readonly IAlarmCService _iAlarmCService;

	public ReceiveController(IAlarmLogService alarmLogService, AlarmCTX alarmCTX, IAlarmCService iAlarmCService)
	{
		_httpClient = new HttpClient();
		_iAlarmLogService = alarmLogService;
		_alarmCtx = alarmCTX;
		_iAlarmCService = iAlarmCService;
	}


	[HttpPost]
	[Route("endpoint")]
	public async Task<IActionResult> ReceiveDataFromApi1([FromBody] IEnumerable<DTOAlarmLog> dtoJournals)
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
					journal.AlarmID = newAlarmC.ID;
				}
				else throw new EntityNotFoundException("There is no AlarmC in the transmitted journal");

				var journalToAdd = new AlarmLog
				{
					HasChanged = journal.HasChanged,
					IRID = journal.IRID,
					AlarmID = journal.AlarmID,
					Station = journal.Station,
					IsAck = false,
					IsActive = journal.IsActive,
					TSRaised = journal.TSRaised,
					TSClear = journal.TSClear,
					Duration = journal.Duration,
					TSRead = null,
					TSGet = journal.TSGet,
				};
				await _iAlarmLogService.AddJournal(journalToAdd);

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
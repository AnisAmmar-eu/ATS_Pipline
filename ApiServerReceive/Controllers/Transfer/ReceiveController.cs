using System.Diagnostics;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Entities.Journals.Services;
using Core.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiServerReceive.Controllers.Transfer;

[Route("api/[controller]")]
[ApiController]
public class ReceiveController : ControllerBase
{
    private readonly AlarmCTX _alarmCtx;


    private readonly HttpClient _httpClient;
    private readonly IJournalServices _iJournalServices;

    public ReceiveController(IJournalServices journalServices, AlarmCTX alarmCTX)
    {
        _httpClient = new HttpClient();
        _iJournalServices = journalServices;
        _alarmCtx = alarmCTX;
    }


    [HttpPost]
    [Route("endpoint")]
    public IActionResult ReceiveDataFromApi1([FromBody] IEnumerable<DTOJournal> dtoJournals)
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
                _iJournalServices.AddJournal(journalToAdd);

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
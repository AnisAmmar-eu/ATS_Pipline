using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTOs;
using Core.Entities.Journals.Services;
using Core.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Transfert.App.Transfert
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveController : ControllerBase
    {


        private readonly HttpClient httpClient;
       private readonly IJournalServices _IJournalServices;
        private readonly AlarmesDbContext _AlarmesDbContext;

        public ReceiveController(IJournalServices IJournalServices, AlarmesDbContext alarmesDbContext)
        {
            httpClient = new HttpClient();
            _IJournalServices = IJournalServices;
            _AlarmesDbContext = alarmesDbContext;
        }




        [HttpPost]
        [Route("endpoint")]
        public IActionResult ReceiveDataFromApi1([FromBody] IEnumerable<DTOJournal> Journals)
        {
            try
            {
                Debug.Print("Reçu depuis l'api 1");

                if (Journals == null || !Journals.Any())
                {
                    return BadRequest("Aucun journal à traiter.");
                }
                string nameTable = "Journal";
                var truncateSql = $"TRUNCATE TABLE {nameTable}";
                int a = _AlarmesDbContext.Database.ExecuteSqlRaw(truncateSql);

                foreach (var journal in Journals)
                {
                    Journal journalToAdd = new Journal
                    {
                        IdAlarme = journal.IdAlarme,
                        Status0 = journal.Status0,
                        Status1 = journal.Status1,
                        TS1 = journal.TS1,
                        TS0 = journal.TS0,
                        TS = journal.TS,
                        Lu = journal.Lu,
                        Station = journal.Station
                    };
                    _IJournalServices.addJournal(journalToAdd);

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
}

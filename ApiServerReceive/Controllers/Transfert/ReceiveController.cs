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
                var truncateSql = $"TRUNCATE TABLE Journal ";
                var truncateSqlTR = $"TRUNCATE TABLE AlarmeTR ";
                _AlarmesDbContext.Database.ExecuteSqlRaw(truncateSql);
                _AlarmesDbContext.Database.ExecuteSqlRaw(truncateSqlTR);


                foreach (var journal in Journals)
                {
                    Journal journalToAdd = new Journal
                    {
                        IdAlarme = journal.IdAlarme,
                        Status1 = journal.Status1,
                        TS1 = journal.TS1,
                        Status0 = journal.Status0,
                        TS0 = journal.TS0,
                        Lu = 0,
                        Station = journal.Station,
                        TSLu = null
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

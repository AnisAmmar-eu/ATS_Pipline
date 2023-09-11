using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using Core.Entities.Journals.Services;
using System.Diagnostics;

namespace AlarmsManagemnt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlarmsController : ControllerBase
    {
        private readonly IJournalServices _IJournalServices;
        public AlarmsController(IJournalServices iJournalServices)
        {
            _IJournalServices = iJournalServices;
        }

        [HttpPost("Collect")]
        public IActionResult Collect()
        {
            return Ok(_IJournalServices.Collect());
        }



        [HttpGet("CollectCyc")]
        public IActionResult CollectCyc(int nbSeconde)
        {          
            return Ok(_IJournalServices.CollectCyc(nbSeconde));
        }

        [HttpGet("GetAllJournal")]
        public IActionResult GetAllJournal()
        {
            return Ok(_IJournalServices.GetAllJournal());
        }


        [HttpPost("LuJournal")]
        public IActionResult LuJournal(int idJournal)
        {
            return Ok(_IJournalServices.LuJournal(idJournal));
        }

    }
}

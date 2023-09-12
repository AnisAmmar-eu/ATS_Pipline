using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Collect()
        {
            return Ok(await _IJournalServices.Collect());
        }



        [HttpGet("CollectCyc")]
        public async Task<IActionResult> CollectCyc(int nbSeconde)
        {          
            return Ok(await _IJournalServices.CollectCyc(nbSeconde));
        }

        [HttpGet("GetAllJournal")]
        public async Task<IActionResult> GetAllJournal()
        {
            return Ok(await _IJournalServices.GetAllJournal());
        }


        [HttpPost("LuJournal/{id}")]
        public async Task<IActionResult> LuJournal([Required] int id)
        {
            return Ok(await _IJournalServices.LuJournal(id));
        }

    }
}

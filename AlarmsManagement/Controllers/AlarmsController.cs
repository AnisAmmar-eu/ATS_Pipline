using System.ComponentModel.DataAnnotations;
using Core.Entities.Journals.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlarmsManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class AlarmsController : ControllerBase
{
    private readonly IJournalServices _iJournalServices;

    public AlarmsController(IJournalServices iJournalServices)
    {
        _iJournalServices = iJournalServices;
    }

    [HttpPost("Collect")]
    public async Task<IActionResult> Collect()
    {
        return Ok(await _iJournalServices.Collect());
    }


    [HttpGet("CollectCyc")]
    public async Task<IActionResult> CollectCyc(int nbSeconde)
    {
        return Ok(await _iJournalServices.CollectCyc(nbSeconde));
    }

    [HttpGet("GetAllJournal")]
    public async Task<IActionResult> GetAllJournal()
    {
        return Ok(await _iJournalServices.GetAllJournal());
    }


    [HttpPost("LuJournal/{id}")]
    public async Task<IActionResult> LuJournal([Required] int id)
    {
        return Ok(await _iJournalServices.ReadJournal(id));
    }
}
using System.ComponentModel.DataAnnotations;
using Core.Entities.Journals.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlarmsManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class AlarmsController : ControllerBase
{
    private readonly IJournalService _iJournalService;

    public AlarmsController(IJournalService iJournalService)
    {
        _iJournalService = iJournalService;
    }

    [HttpPost("Collect")]
    public async Task<IActionResult> Collect()
    {
        return Ok(await _iJournalService.Collect());
    }


    [HttpGet("CollectCyc")]
    public async Task<IActionResult> CollectCyc(int nbSeconde)
    {
        return Ok(await _iJournalService.CollectCyc(nbSeconde));
    }

    [HttpGet("GetAllJournal")]
    public async Task<IActionResult> GetAllJournal()
    {
        return Ok(await _iJournalService.GetAllJournal());
    }


    [HttpPost("LuJournal/{id}")]
    public async Task<IActionResult> LuJournal([Required] int id)
    {
        return Ok(await _iJournalService.ReadJournal(id));
    }
}
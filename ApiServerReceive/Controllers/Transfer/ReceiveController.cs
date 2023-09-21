using System.Diagnostics;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsC.Services;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsLog.Models.DTO.DTOS;
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
	public async Task<IActionResult> ReceiveDataFromApi1([FromBody] IEnumerable<DTOSAlarmLog> dtoAlarmLogs)
	{
		try
		{
			Debug.Print("Reçu depuis l'api 1");

			if (dtoAlarmLogs == null || !dtoAlarmLogs.Any()) return BadRequest("Aucun alarmLog à traiter.");
			var truncateSql = "TRUNCATE TABLE AlarmLog ";
			var truncateSqlRT = "TRUNCATE TABLE AlarmRT ";
			_alarmCtx.Database.ExecuteSqlRaw(truncateSql);
			_alarmCtx.Database.ExecuteSqlRaw(truncateSqlRT);


			foreach (var alarmLog in dtoAlarmLogs)
			{
				DTOAlarmC newAlarmC = await _iAlarmCService.GetByRID(alarmLog.AlarmRID);
				alarmLog.AlarmID = newAlarmC.ID;

				var alarmLogToAdd = new AlarmLog
				{
					HasBeenSent = true,
					AlarmID = alarmLog.AlarmID,
					Station = alarmLog.Station,
					IsAck = false,
					IsActive = alarmLog.IsActive,
					TSRaised = alarmLog.TSRaised,
					TSClear = alarmLog.TSClear,
					Duration = alarmLog.Duration,
					TSRead = null,
					TSGet = alarmLog.TSGet,
					Alarm = newAlarmC.ToModel(),
				};
				await _iAlarmLogService.AddAlarmLog(alarmLogToAdd);

				// _IAlarmLogServices.AddJournalFromPush(alarmLogToAdd);
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
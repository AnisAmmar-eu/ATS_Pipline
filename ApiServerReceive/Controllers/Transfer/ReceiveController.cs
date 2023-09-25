using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsC.Services;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsLog.Models.DTO.DTOS;
using Core.Entities.AlarmsLog.Services;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace ApiServerReceive.Controllers.Transfer;

[Route("api/receive")]
[ApiController]
public class ReceiveController : ControllerBase
{
	private readonly AlarmCTX _alarmCtx;


	private readonly HttpClient _httpClient;
	private readonly IAlarmLogService _alarmLogService;
	private readonly IAlarmCService _alarmCService;
	private readonly IPacketService _packetService;

	public ReceiveController(IAlarmLogService alarmLogService, AlarmCTX alarmCTX, IAlarmCService alarmCService,
		IPacketService packetService)
	{
		_httpClient = new HttpClient();
		_alarmLogService = alarmLogService;
		_alarmCtx = alarmCTX;
		_alarmCService = alarmCService;
		_packetService = packetService;
	}


	[HttpPost]
	[Route("alarm-log")]
	public async Task<IActionResult> ReceiveAlarmLog([FromBody] IEnumerable<DTOSAlarmLog> dtoAlarmLogs)
	{
		try
		{
			Debug.Print("Reçu depuis l'api 1");

			if (dtoAlarmLogs == null || !dtoAlarmLogs.Any()) return BadRequest("Aucun alarmLog à traiter.");
			/*
			var truncateSql = "TRUNCATE TABLE AlarmLog ";
			var truncateSqlRT = "TRUNCATE TABLE AlarmRT ";
			_alarmCtx.Database.ExecuteSqlRaw(truncateSql);
			_alarmCtx.Database.ExecuteSqlRaw(truncateSqlRT);
			*/


			foreach (var alarmLog in dtoAlarmLogs)
			{
				DTOAlarmC newAlarmC = await _alarmCService.GetByRID(alarmLog.AlarmRID);

				AlarmLog alarmLogToAdd = alarmLog.ToModel();
				alarmLogToAdd.ID = 0;
				alarmLogToAdd.IsAck = false;
				alarmLogToAdd.HasBeenSent = true;
				alarmLogToAdd.AlarmID = newAlarmC.ID;
				await _alarmLogService.AddAlarmLog(alarmLogToAdd);
			}

			return Ok(true);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return StatusCode(500, "Erreur interne du serveur");
		}
	}

	[HttpPost("packet")]
	public async Task<IActionResult> ReceivePacket([FromBody] [Required] IEnumerable<DTOPacket> packet)
	{
		try
		{
			await _packetService.ReceivePacket(packet);
			return Ok();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return StatusCode(500, e.Message);
		}
	}
}
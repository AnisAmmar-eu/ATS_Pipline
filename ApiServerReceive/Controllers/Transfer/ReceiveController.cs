using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiServerReceive.Controllers.Transfer;

[Route("api/receive")]
[ApiController]
public class ReceiveController : ControllerBase
{
	private readonly ILogsService _logsService;

	private readonly IAlarmCService _alarmCService;
	private readonly IAlarmLogService _alarmLogService;
	private readonly IPacketService _packetService;
	private readonly IStationCycleService _stationCycleService;

	public ReceiveController(IAlarmLogService alarmLogService, IAlarmCService alarmCService,
		IPacketService packetService, IStationCycleService stationCycleService, ILogsService logsService)
	{
		_alarmLogService = alarmLogService;
		_alarmCService = alarmCService;
		_packetService = packetService;
		_stationCycleService = stationCycleService;
		_logsService = logsService;
	}


	[HttpPost]
	[Route("alarm-log")]
	public async Task<IActionResult> ReceiveAlarmLog([FromBody] [Required] List<DTOSAlarmLog> dtoAlarmLogs)
	{
		try
		{
			foreach (DTOSAlarmLog alarmLog in dtoAlarmLogs)
			{
				DTOAlarmC newAlarmC = await _alarmCService.GetByRID(alarmLog.AlarmRID);

				AlarmLog alarmLogToAdd = alarmLog.ToModel();
				alarmLogToAdd.ID = 0;
				alarmLogToAdd.IsAck = false;
				alarmLogToAdd.HasBeenSent = true;
				alarmLogToAdd.AlarmID = newAlarmC.ID;
				await _alarmLogService.Add(alarmLogToAdd);
			}
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}
		
		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	[HttpPost("packet")]
	public async Task<IActionResult> ReceivePacket([FromBody] [Required] IEnumerable<DTOPacket> packet)
	{
		try
		{
			await _packetService.ReceivePackets(packet);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}

	[HttpPost("station-cycle")]
	public async Task<IActionResult> ReceiveStationCycle([FromBody] [Required] List<DTOStationCycle> dtoStationCycles)
	{
		try
		{
			await _stationCycleService.ReceiveStationCycles(dtoStationCycles);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}
		return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
	}
}
﻿using System.ComponentModel.DataAnnotations;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.DTO.Binders;
using Core.Entities.StationCycles.Services;
using Core.Shared.Attributes;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiServerReceive.Controllers.Transfer;

[ApiController]
[Route("apiServerReceive")]
[ServerAction]
public class ReceiveController : ControllerBase
{
	private readonly IAlarmCService _alarmCService;
	private readonly IAlarmLogService _alarmLogService;
	private readonly ILogService _logService;
	private readonly IPacketService _packetService;
	private readonly IStationCycleService _stationCycleService;

	public ReceiveController(IAlarmLogService alarmLogService, IAlarmCService alarmCService,
		IPacketService packetService, IStationCycleService stationCycleService, ILogService logService)
	{
		_alarmLogService = alarmLogService;
		_alarmCService = alarmCService;
		_packetService = packetService;
		_stationCycleService = stationCycleService;
		_logService = logService;
	}

	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ControllerResponseObject().SuccessResult();
	}

	[HttpPost]
	[Route("alarmsLog")]
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
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}

	[HttpPost("packets")]
	public async Task<IActionResult> ReceivePacket([FromBody] [Required] IEnumerable<DTOPacket> packet)
	{
		try
		{
			await _packetService.ReceivePackets(packet);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}

	/// <summary>
	///     This function will take a furnace packet as argument and will build it. Building it will make it associate
	///     to its stationCycle. StationCycle MUST NOT be marked as sent in the server.
	/// </summary>
	/// <param name="dtoPacket"></param>
	/// <returns></returns>
	[HttpPost("furnacePackets")]
	public async Task<IActionResult> ReceiveFurnacePacketForStationCycle([FromBody] [Required] DTOPacket dtoPacket)
	{
		try
		{
			if (dtoPacket is not DTOFurnace)
				throw new InvalidOperationException("Given packet MUST be a Furnace packet.");
			dtoPacket.ID = 0;
			await _packetService.BuildPacket(dtoPacket.ToModel());
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}

	[HttpPost("stationCycles")]
	public async Task<IActionResult> ReceiveStationCycle(
		[FromBody] [Required] [ModelBinder(typeof(DTOStationCycleListBinder))]
		List<DTOStationCycle> dtoStationCycles)
	{
		try
		{
			await _stationCycleService.ReceiveStationCycles(dtoStationCycles);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}

	[HttpPost("images")]
	public async Task<IActionResult> ReceiveImage()
	{
		try
		{
			FormFileCollection images = new();
			images.AddRange(Request.Form.Files.Where(formFile => formFile.ContentType.Contains("image")));
			await _stationCycleService.ReceiveStationImage(images);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}

	[HttpPost("logs")]
	public async Task<IActionResult> ReceiveLog([FromBody] [Required] List<DTOLog> logs)
	{
		try
		{
			await _logService.ReceiveLogs(logs);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}
}
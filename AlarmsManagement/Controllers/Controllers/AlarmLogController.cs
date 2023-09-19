﻿using System.ComponentModel.DataAnnotations;
using Core.Entities.AlarmsLog.Services;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace AlarmsManagement.Controllers.Controllers;

[ApiController]
[Route("api/alarm-log")]
public class AlarmLogController : ControllerBase
{
	private readonly IAlarmLogService _alarmLogService;

	public AlarmLogController(IAlarmLogService alarmLogService)
	{
		_alarmLogService = alarmLogService;
	}

	// AlarmPLC controller
	[HttpPost("Collect")]
	public async Task<IActionResult> Collect()
	{
		return Ok(await _alarmLogService.Collect());
	}

	// AlarmPLC controller
	[HttpGet("CollectCyc")]
	public async Task<IActionResult> CollectCyc(int nbSecond)
	{
		return Ok(await _alarmLogService.CollectCyc(nbSecond));
	}

	/// <summary>
	/// Get all logs.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAllAlarmLog()
	{
		return new ApiResponseObject(await _alarmLogService.GetAll()).SuccessResult();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route("{alarmClassID}")]
	public async Task<IActionResult> GetAlarmLogByClassID([Required] int alarmClassID)
	{
		return new ApiResponseObject(await _alarmLogService.GetByClassID(alarmClassID)).SuccessResult();
	}


	/// <summary>
	/// Reads a log entry.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPost("{id}")]
	public async Task<IActionResult> ReadAlarmLog([Required] int id)
	{
		return new ApiResponseObject(await _alarmLogService.ReadAlarmLog(id)).SuccessResult();
	}
}
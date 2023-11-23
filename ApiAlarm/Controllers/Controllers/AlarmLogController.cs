using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Controllers.Controllers;

public class AlarmLogController : BaseEndpoint<AlarmLog, DTOAlarmLog, IAlarmLogService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiAlarm/alarmsLog").WithTags(nameof(AlarmLogController));
		MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapGet("{alarmClassID}", GetAlarmLogByClassID);
		group.MapPost("ack", AckAlarmLogs);
	}

	/// <summary>
	///     Get by class ID
	/// </summary>
	/// <param name="alarmClassID"></param>
	/// <param name="alarmLogService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns></returns>
	private static async Task<JsonHttpResult<ApiResponse>> GetAlarmLogByClassID([Required] int alarmClassID,
		IAlarmLogService alarmLogService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await alarmLogService.GetByClassID(alarmClassID), logService,
			httpContext);
	}

	/// <summary>
	///     Ack a list of log entries
	/// </summary>
	/// <param name="alarmLogIDs"></param>
	/// <param name="alarmLogService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns></returns>
	private static async Task<JsonHttpResult<ApiResponse>> AckAlarmLogs([FromBody] [Required] int[] alarmLogIDs,
		IAlarmLogService alarmLogService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await alarmLogService.AckAlarmLogs(alarmLogIDs), logService,
			httpContext);
	}
}
using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarm.Endpoints;

public class AlarmLogEndpoint : BaseEntityEndpoint<AlarmLog, DTOAlarmLog, IAlarmLogService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiAlarm").WithTags(nameof(AlarmLogEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read, nameof(AlarmLog.Alarm));

		group.MapPost("ack", AckAlarmLogs);
	}

	/// <summary>
	///     Ack a list of log entries
	/// </summary>
	/// <param name="alarmLogIDs"></param>
	/// <param name="alarmLogService"></param>
	/// <param name="httpContext"></param>
	private static Task<JsonHttpResult<ApiResponse>> AckAlarmLogs(
		[FromBody][Required] int[] alarmLogIDs,
		IAlarmLogService alarmLogService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => alarmLogService.AckAlarmLogs(alarmLogIDs),
			httpContext);
	}
}
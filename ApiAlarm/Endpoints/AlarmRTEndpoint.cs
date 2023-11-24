using Carter;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiAlarm.Endpoints;

public class AlarmRTEndpoint : BaseEntityEndpoint<AlarmRT, DTOAlarmRT, IAlarmRTService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiAlarm/alarmsRealTime").WithTags(nameof(AlarmRTEndpoint));
		MapBaseEndpoints(group, BaseEndpointFlags.Read, nameof(AlarmRT.Alarm));

		group.MapGet("stats", GetAlarmRTStats);
	}

	/// <summary>
	///     This function returns the statistics of AlarmRT. The result will always be an array of length 3.
	/// </summary>
	/// <returns>
	///     res[0] => Nb No Alarms
	///     res[1] => Nb NonAck
	///     res[2] => Nb Active alarms;
	/// </returns>
	private static async Task<JsonHttpResult<ApiResponse>> GetAlarmRTStats(
		IAlarmRTService alarmRTService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericEndpoint(async () => await alarmRTService.GetAlarmRTStats(), logService, httpContext);
	}
}
﻿using Carter;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Services.System.Logs;

namespace ApiAlarm.Endpoints;

public class TransferEndpoint : BaseEndpoint, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("apiAlarm/transfer/alarmsLog", TransferAlarmsLog);
	}

	private static async Task<IResult> TransferAlarmsLog(
		IAlarmLogService alarmLogService,
		ILogService logService,
		HttpContext httpContext)
	{
		return await GenericEndpointEmptyResponse(
			async () =>
			{
				HttpResponseMessage response = await alarmLogService.SendLogsToServer();
				if (!response.IsSuccessStatusCode)
					throw new(await response.Content.ReadAsStringAsync());
			},
			logService,
			httpContext);
	}
}
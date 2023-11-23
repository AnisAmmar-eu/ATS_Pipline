using Carter;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Services.System.Logs;

namespace ApiAlarm.Controllers.Transfer;

public class TransferController : BaseController, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("apiAlarm/transfer/alarmsLog", TransferAlarmsLog);
	}

	private static async Task<IResult> TransferAlarmsLog(IAlarmLogService alarmLogService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericControllerEmptyResponse(async () =>
		{
			HttpResponseMessage response = await alarmLogService.SendLogsToServer();
			if (!response.IsSuccessStatusCode)
				throw new Exception(await response.Content.ReadAsStringAsync());
		}, logService, httpContext);
	}
}
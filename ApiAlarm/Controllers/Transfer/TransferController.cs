using Carter;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;

namespace ApiAlarm.Controllers.Transfer;

public class TransferController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("apiAlarm/transfer/alarmsLog", TransferAlarmsLog);
	}

	private static async Task<IResult> TransferAlarmsLog(IAlarmLogService alarmLogService, ILogService logService,
		HttpContext httpContext)
	{
		try
		{
			HttpResponseMessage response = await alarmLogService.SendLogsToServer();
			if (response.IsSuccessStatusCode)
				return new ApiResponse().SuccessResult();
			throw new Exception(await response.Content.ReadAsStringAsync());
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext.GetEndpoint(), e);
		}
	}
}
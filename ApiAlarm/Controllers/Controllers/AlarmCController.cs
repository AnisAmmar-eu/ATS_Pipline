using Carter;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;

namespace ApiAlarm.Controllers.Controllers;

public class AlarmCController : BaseEndpoint<AlarmC, DTOAlarmC, IAlarmCService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiAlarm/alarmsClass").WithTags(nameof(AlarmCController));
		MapBaseEndpoints(group, BaseEndpointFlags.Read);
	}
}
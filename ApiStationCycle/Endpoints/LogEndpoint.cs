using Carter;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.DB.System.Logs;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiStationCycle.Endpoints;

public class LogEndpoint : BaseEntityEndpoint<Log, DTOLog, ILogService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(LogEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read | BaseEndpointFlags.Delete);

		group.MapDelete("all", DeleteAll);
	}

	private static Task<JsonHttpResult<ApiResponse>> DeleteAll(ILogService logService, HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(logService.DeleteAllLogs, logService, httpContext);
	}
}
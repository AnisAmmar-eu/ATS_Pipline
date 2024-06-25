using Carter;
using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Entities.DebugsModes.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiStationCycle.Endpoints;

public class DebugModeEndpoint :
	BaseEntityEndpoint<DebugMode, DTODebugMode, IDebugModeService>,
	ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(DebugModeEndpoint));

		// Debug Mode , Logs , Csv 
		group.MapPut("debugMode/{enabled}", ToggleDebugMode);
		group.MapPut("logs/{enabled}", ToggleLogs);
		group.MapPut("setSeverity/{severity}", SetLogSeverity);
		group.MapPut("csvExport/{enabled}", ToggleCsvExport);

		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);
	}

	// Debug Mode , Logs , CSV 
	private static Task<JsonHttpResult<ApiResponse>> ToggleDebugMode(
		bool enabled,
		IDebugModeService debugModeService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => debugModeService.ApplyDebugMode(enabled),
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ToggleLogs(
		bool enabled,
		IDebugModeService debugModeService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => debugModeService.ApplyLog(enabled),
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> SetLogSeverity(
		string severity,
		IDebugModeService debugModeService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => debugModeService.SetSeverity(severity),
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ToggleCsvExport(
		bool enabled,
		IDebugModeService debugModeService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => debugModeService.ApplyCsvExport(enabled),
			httpContext);
	}
}
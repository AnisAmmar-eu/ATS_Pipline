using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiMonitor.Endpoints;

public class MonitorEndpoint : BaseEntityEndpoint<IOTDevice, DTOIOTDevice, IIOTDeviceService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiMonitor").WithTags(nameof(MonitorEndpoint));
		group.MapGet("status", () => new ApiResponse().SuccessResult());
		group.MapPut("reinit", ActiveReinit).WithSummary("Active mode reinit").WithOpenApi();
		group.MapPut("GoMatch/{instanceMatch}/{delay}", TestGoMatch).WithSummary("Test GoMatch").WithOpenApi();

		RouteGroupBuilder groupIOTDevice = MapBaseEndpoints(group, BaseEndpointFlags.Read, nameof(IOTDevice.IOTTags));
		groupIOTDevice.MapGet("status/{rid}", GetStatusByRID).WithSummary("Get a device's STATUS by its RID").WithOpenApi();
		groupIOTDevice.MapPut("rids", GetDevicesByArrayRID).WithSummary("Get devices by their RIDs").WithOpenApi();
	}

	private static Task<JsonHttpResult<ApiResponse>> GetStatusByRID(
		[Required][FromRoute] string rid,
		IIOTDeviceService iotDeviceService,
		ILogService logService,
		HttpContext httpContext) => GenericEndpoint(() => iotDeviceService.GetStatusByRID(rid), logService, httpContext);

	private static Task<JsonHttpResult<ApiResponse>> GetDevicesByArrayRID(
		[FromBody][Required] IEnumerable<string> rids,
		IIOTDeviceService iotDeviceService,
		ILogService logService,
		HttpContext httpContext)
			=> GenericEndpoint(() => iotDeviceService.GetStatusByArrayRID(rids), logService, httpContext);

	private static Task<JsonHttpResult<ApiResponse>> ActiveReinit(
		IIOTDeviceService iotDeviceService,
		ILogService logService,
		HttpContext httpContext) => GenericEndpoint(() => iotDeviceService.ActiveReinit(), logService, httpContext);

	private static Task<JsonHttpResult<ApiResponse>> TestGoMatch(
		[FromBody][Required] IEnumerable<string> origins,
		[Required][FromRoute] int instanceMatch,
		[Required][FromRoute] int delay,
		IToMatchService toMatchService,
		ILogService logService,
		HttpContext httpContext)
			=> GenericEndpoint(() => toMatchService.GoMatch(origins.ToList(), instanceMatch, delay), logService, httpContext);
}
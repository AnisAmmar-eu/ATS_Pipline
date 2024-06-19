using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiMonitor.Endpoints;

public class IOTDeviceEndpoint : BaseEntityEndpoint<IOTDevice, DTOIOTDevice, IIOTDeviceService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiIOT").WithTags(nameof(IOTDeviceEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read, nameof(IOTDevice.IOTTags));

		group.MapGet("status/{rid}", GetStatusByRID).WithSummary("Get a device's STATUS by its RID").WithOpenApi();
		group.MapPut("rids", GetDevicesByArrayRID).WithSummary("Get devices by their RIDs").WithOpenApi();
	}

	private static Task<JsonHttpResult<ApiResponse>> GetStatusByRID(
		[Required][FromRoute] string rid,
		IIOTDeviceService iotDeviceService,
		HttpContext httpContext) => GenericEndpoint(() => iotDeviceService.GetStatusByRID(rid), httpContext);

	private static Task<JsonHttpResult<ApiResponse>> GetDevicesByArrayRID(
		[FromBody][Required] IEnumerable<string> rids,
		IIOTDeviceService iotDeviceService,
		HttpContext httpContext)
			=> GenericEndpoint(() => iotDeviceService.GetStatusByArrayRID(rids), httpContext);
}
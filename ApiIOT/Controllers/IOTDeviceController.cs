using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

public class IOTDeviceController : BaseEndpoint<IOTDevice, DTOIOTDevice, IIOTDeviceService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiIOT/iotDevices").WithTags(nameof(IOTDeviceController));
		MapBaseEndpoints(group, BaseEndpointFlags.Read, nameof(IOTDevice.IOTTags));

		group.MapGet("status/{rid}", GetStatusByRID).WithSummary("Get a device's STATUS by its RID").WithOpenApi();
		group.MapPut("rids", GetDevicesByArrayRID).WithSummary("Get devices by their RIDs").WithOpenApi();
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetStatusByRID([Required] [FromRoute] string rid,
		IIOTDeviceService iotDeviceService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await iotDeviceService.GetStatusByRID(rid), logService, httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetDevicesByArrayRID(
		[FromBody] [Required] IEnumerable<string> rids, IIOTDeviceService iotDeviceService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () => await iotDeviceService.GetStatusByArrayRID(rids), logService,
			httpContext);
	}
}
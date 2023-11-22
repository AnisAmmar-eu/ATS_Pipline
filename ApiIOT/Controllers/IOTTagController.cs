using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

public class IOTTagController : BaseEndpoint<IOTTag, DTOIOTTag, IIOTTagService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiIOT/iotTags").WithTags(nameof(IOTTagController));
		MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapGet("rid/{rid}", GetTagValueByRID).WithSummary("Get a tag by its RID").WithOpenApi();
		group.MapPut("rids", GetTagValueByArrayRID).WithSummary("Get tags by their RIDs").WithOpenApi();
		group.MapPut("setValue/{rid}/{value}", SetTagValueByRID).WithSummary("Set a tag value by its RID")
			.WithOpenApi();
		group.MapPut("setValue", SetTagsValues).WithSummary("Set tags values").WithOpenApi();
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetTagValueByRID([FromRoute] [Required] string rid,
		IIOTTagService iotTagService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await iotTagService.GetByRID(rid), logService, httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetTagValueByArrayRID(
		[FromBody] [Required] IEnumerable<string> rids, IIOTTagService iotTagService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () => await iotTagService.GetByArrayRID(rids), logService, httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> SetTagValueByRID([FromRoute] string rid,
		[FromRoute] string value, IIOTTagService iotTagService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await iotTagService.UpdateTagByRID(rid, value), logService,
			httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> SetTagsValues(
		[FromBody] [Required] List<PatchIOTTag> toUpdate, IIOTTagService iotTagService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () => await iotTagService.UpdateTags(toUpdate), logService, httpContext);
	}
}
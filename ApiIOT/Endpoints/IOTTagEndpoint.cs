using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Endpoints;

public class IOTTagEndpoint : BaseEntityEndpoint<IOTTag, DTOIOTTag, IIOTTagService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiIOT").WithTags(nameof(IOTTagEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapPut("rids", GetTagValueByArrayRID).WithSummary("Get tags by their RIDs").WithOpenApi();
		group.MapPut("setValue/{rid}/{value}", SetTagValueByRID).WithSummary("Set a tag value by its RID")
			.WithOpenApi();
		group.MapPut("setValue", SetTagsValues).WithSummary("Set tags values").WithOpenApi();
	}

	private static Task<JsonHttpResult<ApiResponse>> GetTagValueByArrayRID(
		[FromBody][Required] IEnumerable<string> rids,
		IIOTTagService iotTagService,
		HttpContext httpContext) => GenericEndpoint(() => iotTagService.GetByArrayRID(rids), httpContext);

	private static Task<JsonHttpResult<ApiResponse>> SetTagValueByRID(
		[FromRoute] string rid,
		[FromRoute] string value,
		IIOTTagService iotTagService,
		HttpContext httpContext) => GenericEndpoint(() => iotTagService.UpdateTagByRID(rid, value), httpContext);

	private static Task<JsonHttpResult<ApiResponse>> SetTagsValues(
		[FromBody][Required] List<PatchIOTTag> toUpdate,
		IIOTTagService iotTagService,
		HttpContext httpContext) => GenericEndpoint(() => iotTagService.UpdateTags(toUpdate), httpContext);
}
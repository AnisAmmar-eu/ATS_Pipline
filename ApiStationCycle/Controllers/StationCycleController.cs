using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers;

public class StationCycleController : BaseEndpoint<StationCycle, DTOStationCycle, IStationCycleService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(StationCycleController));
		MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapGet("status", GetStatus);
		group.MapGet("reduced", GetAllRIDs);
		group.MapGet("mostRecent", GetMostRecent);
		group.MapGet("{id}/images/{cameraNb}", GetImageByIdAndCamera);
	}

	private static Ok<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetAllRIDs(IStationCycleService stationCycleService,
		ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await stationCycleService.GetAllRIDs(), logService, httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetMostRecent(IStationCycleService stationCycleService,
		ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () =>
		{
			ReducedStationCycle? result = await stationCycleService.GetMostRecentWithIncludes();
			if (result == null)
				throw new NoDataException("There is no station cycles yet.");
			return result;
		}, logService, httpContext);
	}

	private static async Task<Results<FileContentHttpResult, JsonHttpResult<ApiResponse>>> GetImageByIdAndCamera(
		[Required] [FromRoute] int id, [Required] [FromRoute] int cameraNb, IStationCycleService stationCycleService,
		ILogService logService, HttpContext httpContext)
	{
		byte[] image;
		DateTimeOffset ts;
		try
		{
			FileInfo imageFile = await stationCycleService.GetImagesFromIDAndCamera(id, cameraNb);
			ts = imageFile.CreationTime;
			image = await File.ReadAllBytesAsync(imageFile.FullName);
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext.GetEndpoint(), e);
		}

		httpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
		return TypedResults.File(image, "image/jpeg", ts.ToUnixTimeMilliseconds().ToString());
	}
}
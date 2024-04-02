using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Entities.StationCycles.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Endpoints;

public class StationCycleEndpoint : BaseEntityEndpoint<StationCycle, DTOStationCycle, IStationCycleService>,
	ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(StationCycleEndpoint));
		group.MapGet("status", () => new ApiResponse().SuccessResult());
		if (!Station.IsServer)
			return;

		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapGet("reduced", GetAllRIDs);
		group.MapGet("mostRecent", GetMostRecent);
		group.MapGet("{id:int}/{cameraNb:int}/image", GetImageByIdAndCamera);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetAllRIDs(
		IStationCycleService stationCycleService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(stationCycleService.GetAllRIDs, logService, httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetMostRecent(
		IStationCycleService stationCycleService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () =>
			{
				DTOReducedStationCycle? result = await stationCycleService.GetMostRecentWithIncludes();
				if (result is null)
					throw new NoDataException("There is no station cycles yet.");

				return result;
			},
			logService,
			httpContext);
	}

	private static async Task<Results<FileContentHttpResult, JsonHttpResult<ApiResponse>>> GetImageByIdAndCamera(
		[Required] [FromRoute] int id,
		[Required] [FromRoute] int cameraNb,
		IStationCycleService stationCycleService,
		ILogService logService,
		HttpContext httpContext)
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
			return await new ApiResponse().ErrorResult(logService, httpContext, e);
		}

		httpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
		return TypedResults.File(image, "image/jpeg", ts.ToUnixTimeMilliseconds().ToString());
	}
}
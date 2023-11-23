using Carter;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiVision.Controllers;

public class VisionController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;
		app.MapGroup("apiVision").WithTags(nameof(VisionController)).MapGet("status", GetStatus);
	}

	private static JsonHttpResult<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}
}
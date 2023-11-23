using Carter;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiKPI.Controllers;

public class KPIController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;
		app.MapGroup("apiKPI").WithTags(nameof(KPIController)).MapGet("status", GetStatus);
	}

	private static JsonHttpResult<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}
}
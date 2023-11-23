using Carter;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiAlarm.Controllers.Controllers;

public class AlarmController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("apiAlarm/status", GetStatus);
	}

	private static JsonHttpResult<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}
}
using Carter;
using Core.Shared.Models.ApiResponses;

namespace ApiAlarm.Endpoints;

public class AlarmEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
		=> app.MapGet("apiAlarm/status", () => new ApiResponse().SuccessResult());
}
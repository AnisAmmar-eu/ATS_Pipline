using Carter;
using Core.Shared.Models.ApiResponses;

namespace ApiADS.Endpoints;

public class ADSEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
		=> app.MapGet("apiADS/status", () => new ApiResponse().SuccessResult());
}
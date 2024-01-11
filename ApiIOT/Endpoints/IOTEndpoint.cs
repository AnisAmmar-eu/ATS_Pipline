using Carter;
using Core.Shared.Models.ApiResponses;

namespace ApiIOT.Endpoints;

public class IOTEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
		=> app.MapGroup("apiIOT").WithTags(nameof(IOTEndpoint)).MapGet("status", () => new ApiResponse().SuccessResult());
}
using Carter;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;

namespace ApiVision.Endpoints;

public class VisionEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		app.MapGroup("apiVision").WithTags(nameof(VisionEndpoint)).MapGet("status", () => new ApiResponse().SuccessResult());
	}
}
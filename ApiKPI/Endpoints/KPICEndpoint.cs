using Carter;
using Core.Shared.Dictionaries;
using Core.Shared.Models.ApiResponses;

namespace ApiKPI.Endpoints;

public class KPICEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		app.MapGroup("apiKPI").WithTags(nameof(KPICEndpoint)).MapGet("status", () => new ApiResponse().SuccessResult());
	}
}
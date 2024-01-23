using Carter;
using Core.Shared.Models.ApiResponses;

namespace ApiMonitor.Endpoints;

public class MonitorEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
		 => app.MapGroup("apiMonitor").WithTags(nameof(MonitorEndpoint))
		  .MapGet("status", () => new ApiResponse().SuccessResult());
}
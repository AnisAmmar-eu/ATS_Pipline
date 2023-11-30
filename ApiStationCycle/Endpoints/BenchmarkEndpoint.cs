using Carter;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Endpoints;

public class BenchmarkEndpoint : BaseEntityEndpoint<BenchmarkTest, DTOBenchmarkTest, IBenchmarkTestService>,
	ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiStationCycle/benchmark2")
			.WithTags(nameof(BenchmarkEndpoint));
		MapBaseEndpoints(
			group,
			BaseEndpointFlags.Create | BaseEndpointFlags.Read | BaseEndpointFlags.Update | BaseEndpointFlags.Delete);

		group.MapPost("generate/{nbItems}", GenerateRows);
	}

	private static Task<JsonHttpResult<ApiResponse>> GenerateRows(
		[FromRoute] int nbItems,
		IBenchmarkTestService benchmarkTestService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint( () => benchmarkTestService.GenerateRows(nbItems), logService, httpContext);
	}
}
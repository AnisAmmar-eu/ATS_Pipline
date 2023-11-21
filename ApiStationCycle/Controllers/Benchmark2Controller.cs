using Carter;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;

namespace ApiStationCycle.Controllers;

public class Benchmark2Controller : BaseEndpoint<BenchmarkTest, DTOBenchmarkTest, IBenchmarkTestService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiStationCycle/benchmark2")
			.WithTags(nameof(Benchmark2Controller));
		AddBaseEndpoints(group,
			BaseEndpointFlags.Create | BaseEndpointFlags.Read | BaseEndpointFlags.Update | BaseEndpointFlags.Delete);
	}
}
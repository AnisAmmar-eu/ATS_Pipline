using Carter;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;

namespace ApiStationCycle.Endpoints;

public class ToMatchEndpoint : BaseEntityEndpoint<ToMatch, DTOToMatch, IToMatchService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(ToMatchEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);
	}
}
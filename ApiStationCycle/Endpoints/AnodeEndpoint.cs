using Carter;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.Anodes.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;

namespace ApiStationCycle.Endpoints;

public class AnodeEndpoint : BaseEntityEndpoint<Anode, DTOAnode, IAnodeService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(AnodeEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);
	}
}
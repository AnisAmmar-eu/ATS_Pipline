using Carter;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;

namespace ApiStationCycle.Endpoints;

public class ToSignEndpoint : BaseEntityEndpoint<ToSign, DTOToSign, IToSignService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(ToSignEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);
	}
}
using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.BI.BITemperatures.Services;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Carter;

namespace ApiCamera.Endpoints;

public class TemperatureEndpoint :
	BaseEntityEndpoint<BITemperature, DTOBITemperature, IBITemperatureService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiCamera").WithTags(nameof(CameraApiEndpoint));
		MapBaseEndpoints(group, BaseEndpointFlags.Read);
	}
}
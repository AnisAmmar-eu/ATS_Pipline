using Carter;
using Core.Entities.KPIData.KPIs.Data;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.KPIData.KPIs.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiKPI.Endpoints;

public class KPIEndpoint : BaseEntityEndpoint<KPI, DTOKPI, IKPIService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiKPI").WithTags(nameof(KPIEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapPut("GetAllStationKPI", GetAllStationKPIs)
			.WithSummary("Get all KPI by period, station and origin")
			.WithOpenApi();
	}

	private static Task<JsonHttpResult<ApiResponse>> GetAllStationKPIs(
		[FromBody] KPIrequest kpiRequest,
		IKPIService kpiService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => kpiService.CreateAllStationKPIByPeriod(
				kpiRequest.Start,
				kpiRequest.End,
				kpiRequest.Anodes,
				kpiRequest.OriginStations),
			logService,
			httpContext);
	}
}
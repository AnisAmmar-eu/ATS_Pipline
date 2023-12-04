using Carter;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiKPI.Endpoints;

public class KPIRTEndpoint : BaseEntityEndpoint<KPIRT, DTOKPIRT, IKPIRTService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiKPIRT").WithTags(nameof(KPIRTEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read, nameof(KPIRT.KPIC));

		group.MapPut("{timePeriod}", GetByTimePeriodAndRIDs)
			.WithSummary("Get KPIRT within a single time period by RIDs")
			.WithOpenApi();
	}

	private static Task<JsonHttpResult<ApiResponse>> GetByTimePeriodAndRIDs(
		[FromRoute] string timePeriod,
		[FromBody] List<string> rids,
		IKPIRTService kpirtService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(() => kpirtService.GetByRIDsAndPeriod(timePeriod, rids), logService, httpContext);
	}
}
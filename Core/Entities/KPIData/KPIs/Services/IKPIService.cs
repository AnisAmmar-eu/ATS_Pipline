using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPIData.KPIs.Services;

public interface IKPIService : IBaseEntityService<KPI, DTOKPI>
{
	Task<List<DTOStationKPI>> CreateAllStationKPIByPeriod(
		DateTimeOffset? start,
		DateTimeOffset? end,
		List<string> anodeTypes,
		List<string> stationOrigin);
}
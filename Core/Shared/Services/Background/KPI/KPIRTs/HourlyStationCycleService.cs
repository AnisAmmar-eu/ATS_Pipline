using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRTs;

public class HourlyStationCycleService : BaseHourlyKPIRTService<StationCycle, DTOStationCycle, IStationCycleService,
	DTOStationCycle>
{
	public HourlyStationCycleService(IServiceScopeFactory factory,
		ILogger<BaseHourlyKPIRTService<StationCycle, DTOStationCycle, IStationCycleService, DTOStationCycle>> logger) :
		base(factory, logger)
	{
	}
}
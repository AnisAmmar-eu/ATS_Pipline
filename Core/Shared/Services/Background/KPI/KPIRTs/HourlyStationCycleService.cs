using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRTs;

public class HourlyStationCycleService : BaseHourlyKPIRTService<StationCycle, DTOStationCycle, IStationCycleRepository,
	StationCycle>
{
	public HourlyStationCycleService(IServiceScopeFactory factory,
		ILogger<BaseHourlyKPIRTService<StationCycle, DTOStationCycle, IStationCycleRepository, StationCycle>> logger) :
		base(factory, logger)
	{
	}
}
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.Anodes.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRTs;

public class HourlyAnodeService : BaseHourlyKPIRTService<Anode, DTOAnode, IAnodeRepository,
	Anode>
{
	public HourlyAnodeService(
		IServiceScopeFactory factory,
		ILogger<BaseHourlyKPIRTService<Anode, DTOAnode, IAnodeRepository, Anode>> logger) :
		base(factory, logger)
	{
	}
}
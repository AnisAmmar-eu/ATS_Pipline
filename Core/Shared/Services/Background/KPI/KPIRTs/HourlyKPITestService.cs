using Core.Entities.KPI.KPITests.Models.DB;
using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Entities.KPI.KPITests.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.KPI.KPIRTs;

public class HourlyKPITestService : BaseHourlyKPIRTService<KPITest, DTOKPITest, IKPITestRepository, double>
{
	public HourlyKPITestService(IServiceScopeFactory factory,
		ILogger<BaseHourlyKPIRTService<KPITest, DTOKPITest, IKPITestRepository, double>> logger) : base(factory, logger)
	{
	}
}
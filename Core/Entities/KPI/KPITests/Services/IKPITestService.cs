using Core.Entities.KPI.KPITests.Models.DB;
using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPI.KPITests.Services;

public interface IKPITestService : IServiceBaseEntity<KPITest, DTOKPITest>
{
	
}
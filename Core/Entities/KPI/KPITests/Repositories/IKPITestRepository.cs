using Core.Entities.KPI.KPITests.Models.DB;
using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPI.KPITests.Repositories;

public interface IKPITestRepository : IRepositoryBaseEntity<KPITest, DTOKPITest>
{
	
}
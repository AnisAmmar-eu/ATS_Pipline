using Core.Entities.KPI.KPITests.Models.DB;
using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Entities.KPI.KPITests.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPITests.Services;

public class KPITestService : ServiceBaseEntity<IKPITestRepository, KPITest, DTOKPITest>, IKPITestService
{
	public KPITestService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}
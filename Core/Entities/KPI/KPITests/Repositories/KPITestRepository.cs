using Core.Entities.KPI.KPITests.Models.DB;
using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPI.KPITests.Repositories;

public class KPITestRepository : RepositoryBaseEntity<AnodeCTX, KPITest, DTOKPITest>, IKPITestRepository
{
	public KPITestRepository(AnodeCTX context) : base(context)
	{
	}
}
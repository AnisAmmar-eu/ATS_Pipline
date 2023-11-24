using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPI.KPICs.Repositories;

public class KPICRepository : BaseEntityRepository<AnodeCTX, KPIC, DTOKPIC>, IKPICRepository
{
	public KPICRepository(AnodeCTX context) : base(context)
	{
	}
}
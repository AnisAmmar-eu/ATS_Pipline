using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;

public class KPIRTRepository : RepositoryBaseEntity<AnodeCTX, KPIRT, DTOKPIRT>, IKPIRTRepository
{
	public KPIRTRepository(AnodeCTX context) : base(context)
	{
	}
}
using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPI.KPIEntries.Repositories;

public class KPIEntryRepository : RepositoryBaseEntity<AlarmCTX, KPIEntry, DTOKPIEntry>, IKPIEntryRepository
{
	public KPIEntryRepository(AlarmCTX context) : base(context)
	{
	}
}
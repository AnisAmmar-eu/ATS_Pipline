using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Repositories;

public interface IKPIEntryRepository : IRepositoryBaseEntity<KPIEntry, DTOKPIEntry>
{
	
}
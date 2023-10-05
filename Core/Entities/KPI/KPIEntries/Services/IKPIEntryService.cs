using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services;

public interface IKPIEntryService : IServiceBaseEntity<KPIEntry, DTOKPIEntry>
{
	
}
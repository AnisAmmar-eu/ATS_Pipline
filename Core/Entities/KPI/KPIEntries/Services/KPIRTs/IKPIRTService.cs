using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPIRTs;

public interface IKPIRTService : IServiceBaseEntity<KPIRT, DTOKPIRT>
{
	public Task<List<DTOKPILog>> SaveRTsToLogs(List<string> periodsToSave);
}
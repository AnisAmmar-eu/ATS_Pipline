using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services.KPILogs;

public interface IKPILogService : IServiceBaseEntity<KPILog, DTOKPILog>
{
}
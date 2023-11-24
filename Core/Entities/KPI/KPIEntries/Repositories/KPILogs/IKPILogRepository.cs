using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Repositories.KPILogs;

public interface IKPILogRepository : IBaseEntityRepository<KPILog, DTOKPILog>;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPIEntries.Models.DTO.KPIRTs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;

public interface IKPIRTRepository : IBaseEntityRepository<KPIRT, DTOKPIRT>;
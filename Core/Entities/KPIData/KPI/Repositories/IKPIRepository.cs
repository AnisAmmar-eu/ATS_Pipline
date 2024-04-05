using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.Vision.Dictionaries;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPIData.KPIs.Repositories;

public interface IKPIRepository : IBaseEntityRepository<KPI, DTOKPI>
{
}
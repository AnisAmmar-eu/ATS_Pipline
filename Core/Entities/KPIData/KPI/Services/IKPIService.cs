using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.Dictionaries;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPIData.KPIs.Services;

public interface IKPIService : IBaseEntityService<KPI, DTOKPI>
{
}
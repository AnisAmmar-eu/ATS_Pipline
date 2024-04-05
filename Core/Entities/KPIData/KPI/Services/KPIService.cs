using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Repositories;

namespace Core.Entities.KPIData.KPIs.Services;

public class KPIService : BaseEntityService<IKPIRepository, KPI, DTOKPI>,
    IKPIService
{
    public KPIService(IAnodeUOW anodeUOW) : base(anodeUOW)
    {
    }
}
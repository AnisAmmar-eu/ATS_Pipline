using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.Vision.Dictionaries;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPIData.KPIs.Repositories;

public class KPIRepository : BaseEntityRepository<AnodeCTX, KPI, DTOKPI>,
    IKPIRepository
{
    public KPIRepository(AnodeCTX context) : base(context, [], [])
    {
    }
}
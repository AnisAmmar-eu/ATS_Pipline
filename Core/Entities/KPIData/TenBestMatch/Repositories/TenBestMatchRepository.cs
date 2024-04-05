using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Entities.Vision.Dictionaries;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPIData.TenBestMatchs.Repositories;

public class TenBestMatchRepository : BaseEntityRepository<AnodeCTX, TenBestMatch, DTOTenBestMatch>,
    ITenBestMatchRepository
{
    public TenBestMatchRepository(AnodeCTX context) : base(context, [], [])
    {
    }
}
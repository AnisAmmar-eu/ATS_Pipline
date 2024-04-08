using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Repositories;

public interface ITenBestMatchRepository : IBaseEntityRepository<TenBestMatch, DTOTenBestMatch>
{
}
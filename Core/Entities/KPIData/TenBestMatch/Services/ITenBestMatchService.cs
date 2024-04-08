using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Services;

public interface ITenBestMatchService : IBaseEntityService<TenBestMatch, DTOTenBestMatch>
{
}
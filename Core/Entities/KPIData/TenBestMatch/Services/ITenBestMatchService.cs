using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.Dictionaries;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Services;

public interface ITenBestMatchService : IBaseEntityService<TenBestMatch, DTOTenBestMatch>
{
}
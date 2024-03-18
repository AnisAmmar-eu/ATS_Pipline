using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToLoads;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.ToLoads;

public interface IToLoadService : IBaseEntityService<ToLoad, DTOToLoad>
{
	public Task<ToLoad?[]> LoadNextCycles(IEnumerable<(DataSetID, TimeSpan)> datasets);
}
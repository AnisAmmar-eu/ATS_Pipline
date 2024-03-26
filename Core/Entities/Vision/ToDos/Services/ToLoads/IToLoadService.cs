using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToLoads;

public interface IToLoadService : IBaseEntityService<ToLoad, DTOToLoad>
{
	public Task<ToLoad?[]> LoadNextCycles(IEnumerable<(DataSetID, TimeSpan)> datasets);
}
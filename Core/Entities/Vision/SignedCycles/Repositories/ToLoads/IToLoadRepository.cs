using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToLoads;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Repositories.ToLoads;

public interface IToLoadRepository : IBaseEntityRepository<ToLoad, DTOToLoad>
{
	public Task<ToLoad?> Peek(DataSetID dataSetID);
}
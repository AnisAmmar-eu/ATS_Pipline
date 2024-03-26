using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Repositories.ToLoads;

public interface IToLoadRepository : IBaseEntityRepository<ToLoad, DTOToLoad>
{
	public Task<ToLoad?> Peek(DataSetID dataSetID);
}
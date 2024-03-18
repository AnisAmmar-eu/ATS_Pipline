using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.ToMatchs;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToMatchs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Repositories.ToMatchs;

public interface IToMatchRepository : IBaseEntityRepository<ToMatch, DTOToMatch>
{
	public Task<ToMatch?> Peek(DataSetID dataSetID);
}
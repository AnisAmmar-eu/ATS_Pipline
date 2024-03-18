using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;
using Core.Entities.Vision.SignedCycles.Models.DB.ToMatchs;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToMatchs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.ToLoads;

public interface IToMatchService : IBaseEntityService<ToMatch, DTOToMatch>
{
	public Task MatchNextCycles(IEnumerable<(DataSetID, TimeSpan, ToLoad?)> datasets);
}
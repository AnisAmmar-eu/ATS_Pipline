using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToLoads;

public interface IToMatchService : IBaseEntityService<ToMatch, DTOToMatch>
{
	public Task MatchNextCycles(IEnumerable<(DataSetID, TimeSpan, ToLoad?)> datasets);
}
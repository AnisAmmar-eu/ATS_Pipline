using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.MatchableStacks;

public interface IMatchableStackService : IBaseEntityService<MatchableStack, DTOMatchableStack>
{
	public Task<MatchableStack?> Peek();
	public Task<MatchableStack?> Peek(DataSetID dataSetID);
	public Task MatchNextCycle(MatchableStack? matchable, LoadableQueue? loadable, TimeSpan delay);
}
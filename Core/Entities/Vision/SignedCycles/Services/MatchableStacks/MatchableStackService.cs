using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Repositories.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Services.LoadableQueues;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.MatchableStacks;

public class MatchableStackService : BaseEntityService<IMatchableStackRepository, MatchableStack, DTOMatchableStack>,
	IMatchableStackService
{
	private readonly ILoadableQueueService _loadableQueueService;

	public MatchableStackService(IAnodeUOW anodeUOW, ILoadableQueueService loadableQueueService) : base(anodeUOW)
	{
		_loadableQueueService = loadableQueueService;
	}

	public async Task<MatchableStack?> Peek()
	{
		try
		{
			return await AnodeUOW.MatchableStack.GetBy(
				orderBy: query => query.OrderByDescending(queue => queue.CycleTS),
				includes: nameof(MatchableStack.MatchableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}

	public async Task<MatchableStack?> Peek(DataSetID dataSetID)
	{
		try
		{
			return await AnodeUOW.MatchableStack.GetBy(
				filters: [loadable => loadable.DataSetID == dataSetID],
				orderBy: query => query.OrderByDescending(queue => queue.CycleTS),
				includes: nameof(MatchableStack.MatchableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}

	public async Task MatchNextCycle(MatchableStack? matchable, LoadableQueue? loadable, TimeSpan delay)
	{
		if (matchable is null)
			return;
		// Verifies if it is too early or not to match the cycle.
		if (loadable is null || matchable.CycleTS.Add(delay) > loadable.CycleTS)
			return;
		// TODO Load in Vision.dll
		Console.WriteLine(
			$"Matching following cycle {matchable.MatchableCycle.RID} at {DateTimeOffset.Now.ToString()}");
		// TODO Assign to anode etc...
		await AnodeUOW.StartTransaction();
		AnodeUOW.MatchableStack.Remove(matchable);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}
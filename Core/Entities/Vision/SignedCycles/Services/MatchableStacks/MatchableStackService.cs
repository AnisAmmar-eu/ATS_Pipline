using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Repositories.MatchableStacks;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.MatchableStacks;

public class MatchableStackService : BaseEntityService<IMatchableStackRepository, MatchableStack, DTOMatchableStack>,
	IMatchableStackService
{
	public MatchableStackService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task MatchNextCycles(IEnumerable<(DataSetID, TimeSpan, LoadableQueue?)> datasets)
	{
		List<(MatchableStack?, TimeSpan, LoadableQueue?)> matchables = [];
		foreach ((DataSetID, TimeSpan, LoadableQueue?) valueTuple in datasets)
            matchables.Add((await AnodeUOW.MatchableStack.Peek(valueTuple.Item1), valueTuple.Item2, valueTuple.Item3));

		MatchableStack?[] toUpdate = await Task.WhenAll(
			matchables.Select(tuple => MatchCycle(tuple.Item1, tuple.Item3, tuple.Item2)));
		await AnodeUOW.StartTransaction();
		toUpdate.Where(matchable => matchable is not null)!
			.ToList<MatchableStack>()
			.ForEach(matchable => AnodeUOW.MatchableStack.Remove(matchable));
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	private static async Task<MatchableStack?> MatchCycle(
		MatchableStack? matchable,
		LoadableQueue? loadable,
		TimeSpan delay)
	{
		if (matchable is null)
			return null;
		// Verifies if it is too early or not to match the cycle.
		if (loadable is not null && matchable.CycleTS.Add(delay) > loadable.CycleTS)
			return null;
		// TODO Load in Vision.dll
		Console.WriteLine("======================\n\n"
			+ $"Matching following cycle {matchable.MatchableCycle.RID} at {DateTimeOffset.Now.ToString()}"
			+ "\n\n=======================");
		await Task.Delay(100);
		// TODO Assign to anode etc...
		return matchable;
	}
}
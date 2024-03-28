using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Entities.Vision.ToDos.Repositories.ToMatchs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToMatchs;

public class ToMatchService : BaseEntityService<IToMatchRepository, ToMatch, DTOToMatch>,
	IToMatchService
{
	public ToMatchService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task MatchNextCycles(IEnumerable<(int, TimeSpan, ToLoad?)> datasets)
	{
		List<(ToMatch?, TimeSpan, ToLoad?)> matchables = [];
		foreach ((int, TimeSpan, ToLoad?) valueTuple in datasets)
            matchables.Add((await AnodeUOW.ToMatch.Peek(valueTuple.Item1), valueTuple.Item2, valueTuple.Item3));

		ToMatch?[] toUpdate = await Task.WhenAll(
			matchables.Select(tuple => MatchCycle(tuple.Item1, tuple.Item3, tuple.Item2)));
		await AnodeUOW.StartTransaction();
		toUpdate.Where(matchable => matchable is not null)!
			.ToList<ToMatch>()
			.ForEach(matchable => AnodeUOW.ToMatch.Remove(matchable));
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	private static async Task<ToMatch?> MatchCycle(
		ToMatch? matchable,
		ToLoad? loadable,
		TimeSpan delay)
	{
		if (matchable is null)
			return null;
		// Verifies if it is too early or not to match the cycle.
		if (loadable is not null && matchable.ShootingTS + delay > loadable.ShootingTS)
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
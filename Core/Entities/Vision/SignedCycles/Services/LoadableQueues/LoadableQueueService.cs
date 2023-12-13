using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Repositories.LoadableQueues;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.LoadableQueues;

public class LoadableQueueService : BaseEntityService<ILoadableQueueRepository, LoadableQueue, DTOLoadableQueue>,
	ILoadableQueueService
{
	public LoadableQueueService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<LoadableQueue?[]> LoadNextCycles(IEnumerable<(DataSetID, TimeSpan)> datasets)
	{
		List<(LoadableQueue?, TimeSpan)> loadables = [];
		foreach ((DataSetID, TimeSpan) valueTuple in datasets)
            loadables.Add((await AnodeUOW.LoadableQueue.Peek(valueTuple.Item1), valueTuple.Item2));

		LoadableQueue?[] toUpdate = await Task.WhenAll(loadables.Select(tuple => LoadCycle(tuple.Item1, tuple.Item2)));
		await AnodeUOW.StartTransaction();
		toUpdate.Where(loadable => loadable is not null)!
			.ToList<LoadableQueue>()
			.ForEach(loadable => AnodeUOW.LoadableQueue.Remove(loadable));
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return toUpdate;
	}

	private static async Task<LoadableQueue?> LoadCycle(LoadableQueue? loadable, TimeSpan delay)
	{
		if (loadable is null)
			return null;
		// Verifies if it is too early or not to load the cycle.
		if (loadable.CycleTS.Add(delay) > DateTimeOffset.Now)
			return null;
		// TODO Load in Vision.dll
		Console.WriteLine("=========================\n\n"
			+ $"Loading following cycle {loadable.LoadableCycle.RID} at {DateTimeOffset.Now.ToString()}"
			+ "\n\n=================================");
		await Task.Delay(100);
		return loadable;
	}
}
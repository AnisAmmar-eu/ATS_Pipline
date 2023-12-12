using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Repositories.LoadableQueues;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.LoadableQueues;

public class LoadableQueueService : BaseEntityService<ILoadableQueueRepository, LoadableQueue, DTOLoadableQueue>,
	ILoadableQueueService
{
	public LoadableQueueService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<LoadableQueue?> Peek(DataSetID dataSetID)
	{
		try
		{
			return await AnodeUOW.LoadableQueue.GetBy(
				filters: [loadable => loadable.DataSetID == dataSetID],
				orderBy: query => query.OrderBy(queue => queue.CycleTS),
				includes: nameof(LoadableQueue.LoadableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
		catch (Exception)
		{
			Console.WriteLine("ofzbfzneofgnz");
			return null;
		}
	}

	public async Task LoadNextCycle(LoadableQueue? loadable, TimeSpan delay)
	{
		if (loadable is null)
			return;
		// Verifies if it is too early or not to load the cycle.
		if (loadable.CycleTS.Add(delay) < DateTimeOffset.Now)
			return;
		// TODO Load in Vision.dll
		Console.WriteLine($"Loading following cycle {loadable.LoadableCycle.RID} at {DateTimeOffset.Now.ToString()}");
		await AnodeUOW.StartTransaction();
		AnodeUOW.LoadableQueue.Remove(loadable);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}
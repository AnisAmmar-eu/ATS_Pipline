using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Entities.Vision.ToDos.Repositories.ToLoads;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToLoads;

public class ToLoadService : BaseEntityService<IToLoadRepository, ToLoad, DTOToLoad>,
	IToLoadService
{
	public ToLoadService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<ToLoad?[]> LoadNextCycles(IEnumerable<(DataSetID, TimeSpan)> datasets)
	{
		List<(ToLoad?, TimeSpan)> loadables = [];
		foreach ((DataSetID, TimeSpan) valueTuple in datasets)
            loadables.Add((await AnodeUOW.ToLoad.Peek(valueTuple.Item1), valueTuple.Item2));

		ToLoad?[] toUpdate = await Task.WhenAll(loadables.Select(tuple => LoadCycle(tuple.Item1, tuple.Item2)));
		await AnodeUOW.StartTransaction();
		toUpdate.Where(loadable => loadable is not null)!
			.ToList<ToLoad>()
			.ForEach(loadable => AnodeUOW.ToLoad.Remove(loadable));
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return toUpdate;
	}

	private static async Task<ToLoad?> LoadCycle(ToLoad? loadable, TimeSpan delay)
	{
		if (loadable is null)
			return null;
		// Verifies if it is too early or not to load the cycle.
		if (loadable.ShootingTS + delay > DateTimeOffset.Now)
			return null;
		// TODO Load in Vision.dll
		Console.WriteLine("=========================\n\n"
			+ $"Loading following cycle {loadable.LoadableCycle.RID} at {DateTimeOffset.Now.ToString()}"
			+ "\n\n=================================");
		await Task.Delay(100);
		return loadable;
	}
}
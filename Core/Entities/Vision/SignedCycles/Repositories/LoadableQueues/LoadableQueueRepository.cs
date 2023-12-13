using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.SignedCycles.Repositories.LoadableQueues;

public class LoadableQueueRepository : BaseEntityRepository<AnodeCTX, LoadableQueue, DTOLoadableQueue>,
	ILoadableQueueRepository
{
	public LoadableQueueRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<LoadableQueue?> Peek(DataSetID dataSetID)
	{
		try
		{
			return await GetBy(
				filters: [loadable => loadable.DataSetID == dataSetID],
				orderBy: query => query.OrderBy(loadable => loadable.CycleTS),
				withTracking: false,
				includes: nameof(LoadableQueue.LoadableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}
}
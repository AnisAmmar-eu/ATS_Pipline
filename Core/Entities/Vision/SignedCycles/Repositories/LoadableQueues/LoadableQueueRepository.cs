using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.SignedCycles.Repositories.LoadableQueues;

public class LoadableQueueRepository : BaseEntityRepository<AnodeCTX, LoadableQueue, DTOLoadableQueue>,
	ILoadableQueueRepository
{
	public LoadableQueueRepository(AnodeCTX context) : base(context)
	{
	}
}
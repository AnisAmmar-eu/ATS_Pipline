using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Services.LoadableQueues;

public interface ILoadableQueueService : IBaseEntityService<LoadableQueue, DTOLoadableQueue>
{
	public Task<LoadableQueue> Peek(DataSetID dataSetID);
	public Task LoadNextCycle(DataSetID dataSetID, TimeSpan delay);
}
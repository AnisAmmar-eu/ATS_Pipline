using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Repositories.LoadableQueues;

public interface ILoadableQueueRepository : IBaseEntityRepository<LoadableQueue, DTOLoadableQueue>;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;

public partial class DTOLoadableQueue : DTOSignedCycle, IDTO<LoadableQueue, DTOLoadableQueue>
{
	public DTOLoadableQueue()
	{
	}

	public DTOLoadableQueue(LoadableQueue loadableQueue) : base(loadableQueue)
	{
		LoadableCycleID = loadableQueue.LoadableCycleID;
		LoadableCycle = loadableQueue.LoadableCycle;
	}

	public override LoadableQueue ToModel()
	{
		return new(this);
	}
}
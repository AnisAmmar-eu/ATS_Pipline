using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;

namespace Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;

public partial class LoadableQueue
{
	public LoadableQueue()
	{
	}

	public LoadableQueue(DTOLoadableQueue dtoLoadableQueue) : base(dtoLoadableQueue)
	{
		LoadableCycleID = dtoLoadableQueue.LoadableCycleID;
		if (dtoLoadableQueue.LoadableCycle is not null)
			LoadableCycle = dtoLoadableQueue.LoadableCycle;
	}

	public override DTOLoadableQueue ToDTO()
	{
		return new(this);
	}
}
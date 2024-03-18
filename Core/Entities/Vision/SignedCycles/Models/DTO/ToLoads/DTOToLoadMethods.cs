using Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.ToLoads;

public partial class DTOToLoad : DTOSignedCycle, IDTO<ToLoad, DTOToLoad>
{
	public DTOToLoad()
	{
	}

	public DTOToLoad(ToLoad loadableQueue) : base(loadableQueue)
	{
		LoadableCycleID = loadableQueue.LoadableCycleID;
		LoadableCycle = loadableQueue.LoadableCycle;
	}

	public override ToLoad ToModel()
	{
		return new(this);
	}
}
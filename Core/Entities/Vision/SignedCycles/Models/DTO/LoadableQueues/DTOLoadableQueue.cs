using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;

public partial class DTOLoadableQueue : DTOSignedCycle, IDTO<LoadableQueue, DTOLoadableQueue>
{
	public int LoadableCycleID { get; set; }
	public LoadableCycle? LoadableCycle { get; set; }
}
using Core.Entities.StationCycles.Models.DTO.SigningCycles;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles;

public abstract partial class LoadableCycle : StationCycle, IBaseEntity<LoadableCycle, DTOSigningCycle>
{
	public List<LoadableQueue> LoadableQueues { get; set; } = [];
}
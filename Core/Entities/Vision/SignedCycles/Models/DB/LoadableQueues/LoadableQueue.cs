using System.Data;
using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DTO.LoadableQueues;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;

public partial class LoadableQueue : SignedCycle, IBaseEntity<LoadableQueue, DTOLoadableQueue>
{
	public int LoadableCycleID { get; set; }

	public LoadableCycle LoadableCycle
	{
		set => _loadableCycle = value;
		get => _loadableCycle
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(LoadableCycle));
	}

	private LoadableCycle? _loadableCycle;
}
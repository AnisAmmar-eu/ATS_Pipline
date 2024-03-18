using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToLoads;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;

public partial class ToLoad : SignedCycle, IBaseEntity<ToLoad, DTOToLoad>
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
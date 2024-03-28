using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToLoads;

public partial class ToLoad : ToDo, IBaseEntity<ToLoad, DTOToLoad>
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
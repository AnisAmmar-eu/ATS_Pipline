using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

public partial class ToMatch : ToDo, IBaseEntity<ToMatch, DTOToMatch>
{
	public int MatchableCycleID { get; set; }
	public DataSetID DataSetID { get; set; }

	public MatchableCycle MatchableCycle
	{
		set => _matchableCycle = value;
		get => _matchableCycle
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(MatchableCycle));
	}

	private MatchableCycle? _matchableCycle;
}
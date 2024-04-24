using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles;

public abstract partial class MatchableCycle : StationCycle, IBaseEntity<MatchableCycle, DTOMatchingCycle>
{
	public SignMatchStatus MatchingCamera1 { get; set; }
	public SignMatchStatus MatchingCamera2 { get; set; }
	public DateTimeOffset? MatchingTS { get; set; }
	public int? KPIID { get; set; }

	private KPI? _kpi;

	public KPI KPI
	{
		set => _kpi = value;
		get => _kpi
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(KPI));
	}
}
using Core.Entities.KPIs.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles;

public abstract partial class MatchableCycle : StationCycle, IBaseEntity<MatchableCycle, DTOMatchingCycle>
{
	public SignMatchStatus MatchingCamera1 { get; set; }
	public SignMatchStatus MatchingCamera2 { get; set; }
	public DateTimeOffset? MatchingTS { get; set; }
	public KPI KPI { get; set; }
}
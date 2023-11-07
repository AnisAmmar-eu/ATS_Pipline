using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Interfaces;
using Core.Entities.StationCycles.Models.DTO.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S5Cycles;

public partial class S5Cycle : StationCycle, IBaseEntity<S5Cycle, DTOS5Cycle>, IMatchableCycle
{
	public SignMatchStatus MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus MatchingCamera2 { get; set; } = SignMatchStatus.NA;
	public new AnodeDX? Anode { get; set; }
}
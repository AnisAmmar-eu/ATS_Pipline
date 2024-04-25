using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;

public partial class S5Cycle : MatchableCycle, IBaseEntity<S5Cycle, DTOS5Cycle>
{
	new public AnodeDX? Anode { get; set; }
	public SignMatchStatus ChainMatchingCamera1 { get; set; }
	public SignMatchStatus ChainMatchingCamera2 { get; set; }
}
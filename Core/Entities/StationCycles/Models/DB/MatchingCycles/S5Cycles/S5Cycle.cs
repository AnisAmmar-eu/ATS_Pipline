using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.MatchingCycles.S5Cycles;

public partial class S5Cycle : MatchingCycle, IBaseEntity<S5Cycle, DTOS5Cycle>
{
	public new AnodeDX? Anode { get; set; }
}
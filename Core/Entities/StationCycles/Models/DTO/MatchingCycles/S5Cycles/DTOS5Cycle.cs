using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;

public partial class DTOS5Cycle : DTOMatchingCycle, IDTO<S5Cycle, DTOS5Cycle>
{
	public SignMatchStatus ChainMatchingCamera1 { get; set; }
	public SignMatchStatus ChainMatchingCamera2 { get; set; }
}
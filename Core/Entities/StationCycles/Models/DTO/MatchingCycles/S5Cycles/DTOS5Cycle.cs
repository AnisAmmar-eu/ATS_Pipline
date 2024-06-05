using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;

public partial class DTOS5Cycle : DTOMatchingCycle, IDTO<S5Cycle, DTOS5Cycle>
{
	new public string CycleType { get; set; } = CycleTypes.S5;
	public SignMatchStatus ChainMatchingCamera1 { get; set; }
	public SignMatchStatus ChainMatchingCamera2 { get; set; }
	public bool ChainMatchingResult { get; set; }
	public int? ChainCycleID { get; set; }
	public DTOS3S4Cycle? ChainCycle { get; set; }
}
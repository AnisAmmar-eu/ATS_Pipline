using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Interfaces;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.S5Cycles;

public partial class DTOS5Cycle : DTOStationCycle, IDTO<S5Cycle, DTOS5Cycle>, IMatchableCycle
{
	public SignMatchStatus MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus MatchingCamera2 { get; set; } = SignMatchStatus.NA;
}
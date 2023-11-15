using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.MatchingCycles;

public partial class MatchingCycle : StationCycle, IBaseEntity<MatchingCycle, DTOMatchingCycle>
{
	public SignMatchStatus MatchingCamera1 { get; set; }
	public SignMatchStatus MatchingCamera2 { get; set; }
}
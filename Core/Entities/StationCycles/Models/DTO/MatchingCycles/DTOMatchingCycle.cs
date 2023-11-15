using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchingCycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles;

public partial class DTOMatchingCycle : DTOStationCycle, IDTO<MatchingCycle, DTOMatchingCycle>
{
	public SignMatchStatus MatchingCamera1 { get; set; }
	public SignMatchStatus MatchingCamera2 { get; set; }
}
using System.Text.Json.Serialization;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles;

[JsonDerivedType(typeof(DTOS3S4Cycle))]
[JsonDerivedType(typeof(DTOS5Cycle))]
public partial class DTOMatchingCycle : DTOStationCycle, IDTO<MatchableCycle, DTOMatchingCycle>
{
	public SignMatchStatus MatchingCamera1 { get; set; }
	public SignMatchStatus MatchingCamera2 { get; set; }
	public DateTimeOffset? MatchingTS { get; set; }
}
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.S5Cycles;

public partial class DTOS5Cycle : DTOStationCycle, IDTO<S5Cycle, DTOS5Cycle>
{
	public int MatchingCamera1 { get; set; } = (int)SignMatchStatus.NA;
	public int MatchingCamera2 { get; set; } = (int)SignMatchStatus.NA;
}
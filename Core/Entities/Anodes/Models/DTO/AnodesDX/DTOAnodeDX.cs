using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO.AnodesDX;

public partial class DTOAnodeDX : DTOAnode, IDTO<AnodeDX, DTOAnodeDX>
{
	public int? S5CycleID { get; set; }
	public DateTimeOffset? S5CycleTS { get; set; }
	public DTOS5Cycle? S5Cycle { get; set; }
}
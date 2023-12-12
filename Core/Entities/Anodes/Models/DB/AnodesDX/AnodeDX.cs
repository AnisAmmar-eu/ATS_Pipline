using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX : Anode, IBaseEntity<AnodeDX, DTOAnodeDX>
{
	public int? S5CycleID { get; set; }
	public DateTimeOffset? S5CycleTS { get; set; }
	public S5Cycle? S5Cycle { get; set; }
}
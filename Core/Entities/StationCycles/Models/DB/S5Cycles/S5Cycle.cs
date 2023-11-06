using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Models.DTO.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S5Cycles;

public partial class S5Cycle : StationCycle, IBaseEntity<S5Cycle, DTOS5Cycle>
{
	public new AnodeDX? Anode { get; set; }
}
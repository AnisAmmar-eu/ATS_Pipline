using Core.Entities.StationCycles.Models.DTO.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S5Cycles;

public partial class S5Cycle : StationCycle, IBaseEntity<S5Cycle, DTOS5Cycle>
{
	public override DTOS5Cycle ToDTO()
	{
		return new DTOS5Cycle(this);
	}
}
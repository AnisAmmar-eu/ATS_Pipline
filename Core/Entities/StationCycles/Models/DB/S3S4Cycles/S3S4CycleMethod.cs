using Core.Entities.StationCycles.Models.DTO.S3S4Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S3S4Cycles;

public partial class S3S4Cycle : StationCycle, IBaseEntity<S3S4Cycle, DTOS3S4Cycle>
{
	public override DTOS3S4Cycle ToDTO()
	{
		return new DTOS3S4Cycle(this);
	}
}
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle : BaseEntity, IBaseEntity<StationCycle, DTOStationCycle>
{
	public override DTOStationCycle ToDTO()
	{
		return new DTOStationCycle(this);
	}
}
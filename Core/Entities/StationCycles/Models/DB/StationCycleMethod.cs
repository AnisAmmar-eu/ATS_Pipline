using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle : BaseEntity, IBaseEntity<StationCycle, DTOStationCycle>
{
	protected StationCycle()
	{
	}

	public override DTOStationCycle ToDTO()
	{
		return new DTOStationCycle(this);
	}

	public static StationCycle Create()
	{
		if (Station.Type == StationType.S1S2)
			return new S1S2Cycle();
		if (Station.Type == StationType.S3S4)
			return new S3S4Cycle();
		return new S5Cycle();
	}
}
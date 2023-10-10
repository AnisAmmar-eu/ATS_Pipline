using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.StationCycles.Repositories;

public class StationCycleRepository : RepositoryBaseEntity<AnodeCTX, StationCycle, DTOStationCycle>, IStationCycleRepository
{
	public StationCycleRepository(AnodeCTX context) : base(context)
	{
	}
}
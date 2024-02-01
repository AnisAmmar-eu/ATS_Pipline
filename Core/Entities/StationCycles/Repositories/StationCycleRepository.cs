using System.Linq.Expressions;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.StationCycles.Repositories;

public class StationCycleRepository : BaseEntityRepository<AnodeCTX, StationCycle, DTOStationCycle>,
	IStationCycleRepository
{
	public StationCycleRepository(AnodeCTX context) : base(context)
	{
	}

	public Task<List<StationCycle>> GetAllWithIncludes(
		Expression<Func<StationCycle, bool>>[]? filters = null,
		Func<IQueryable<StationCycle>, IOrderedQueryable<StationCycle>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null)
	{
		return GetAll(
			filters,
			orderBy,
			withTracking,
			maxCount,
			nameof(StationCycle.ShootingPacket),
			nameof(StationCycle.AlarmListPacket),
			nameof(S3S4Cycle.InFurnacePacket),
			nameof(S3S4Cycle.OutFurnacePacket));
	}
}
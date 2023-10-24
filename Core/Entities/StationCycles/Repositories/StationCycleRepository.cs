using System.Linq.Expressions;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.StationCycles.Repositories;

public class StationCycleRepository : RepositoryBaseEntity<AnodeCTX, StationCycle, DTOStationCycle>,
	IStationCycleRepository
{
	public StationCycleRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<List<StationCycle>> GetAllWithIncludes(
		Expression<Func<StationCycle, bool>>[]? filters = null,
		Func<IQueryable<StationCycle>, IOrderedQueryable<StationCycle>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null)
	{
		return await GetAll(filters, orderBy, withTracking, maxCount, includes: new[]
			{
				"AnnouncementPacket", "DetectionPacket", "ShootingPacket", "AlarmListPacket", "InFurnacePacket",
				"OutFurnacePacket"
			});
	}
}
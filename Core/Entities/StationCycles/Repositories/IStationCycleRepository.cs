using System.Linq.Expressions;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Repositories;

public interface IStationCycleRepository : IBaseEntityRepository<StationCycle, DTOStationCycle>
{
	public Task<List<StationCycle>> GetAllWithIncludes(
		Expression<Func<StationCycle, bool>>[]? filters = null,
		Func<IQueryable<StationCycle>, IOrderedQueryable<StationCycle>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null);
}
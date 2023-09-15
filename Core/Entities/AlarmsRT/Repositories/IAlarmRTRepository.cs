using System.Linq.Expressions;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Entities.AlarmsRT.Models.DB;
using Core.Entities.AlarmsRT.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.AlarmsRT.Repositories;

public interface IAlarmRTRepository : IRepositoryBaseEntity<AlarmRT, DTOAlarmRT>
{
	public Task<List<AlarmRT>> GetAllWithInclude(
		Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null,
		bool withTracking = true);
}
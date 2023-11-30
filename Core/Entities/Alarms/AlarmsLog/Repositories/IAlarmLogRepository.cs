using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Repositories;

public interface IAlarmLogRepository : IBaseEntityRepository<AlarmLog, DTOAlarmLog>
{
	public Task<AlarmLog> GetByIdWithIncludes(
		int id,
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		bool withTracking = true);

	public Task<AlarmLog> GetByWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true);

	public Task<List<AlarmLog>> GetAllWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true);
}
using System.Linq.Expressions;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Repositories;

public interface IAlarmLogRepository : IRepositoryBaseEntity<AlarmLog, DTOAlarmLog>
{
	public Task<AlarmLog> GetByIdWithIncludes(int id, Expression<Func<AlarmLog, bool>>[]? filters = null,
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
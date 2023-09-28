using System.Linq.Expressions;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
namespace Core.Entities.Alarms.AlarmsLog.Repositories;

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
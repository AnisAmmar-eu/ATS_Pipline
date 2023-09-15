using System.Linq.Expressions;
using Core.Entities.AlarmsLog.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Repositories;

public class AlarmLogRepository : RepositoryBaseEntity<AlarmCTX, AlarmLog, DTOAlarmLog>, IAlarmLogRepository
{
	public AlarmLogRepository(AlarmCTX context) : base(context)
	{
	}

	public async Task<AlarmLog> GetByIdWithIncludes(int id, Expression<Func<AlarmLog, bool>>[]? filters = null,
		bool withTracking = true)
	{
		return await GetById(id, filters, withTracking, includes: new[] { "Alarm" });
	}

	public async Task<AlarmLog> GetByWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true)
	{
		return await GetBy(filters, orderBy, withTracking, includes: new[] { "Alarm" });
	}

	public async Task<List<AlarmLog>> GetAllWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true)
	{
		return await GetAll(filters, orderBy, withTracking, includes: new[] { "Alarm" });
	}
}
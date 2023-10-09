using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarms.AlarmsLog.Repositories;

public class AlarmLogRepository : RepositoryBaseEntity<AnodeCTX, AlarmLog, DTOAlarmLog>, IAlarmLogRepository
{
	public AlarmLogRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<AlarmLog> GetByIdWithIncludes(int id, Expression<Func<AlarmLog, bool>>[]? filters = null,
		bool withTracking = true)
	{
		return await GetById(id, filters, withTracking, "Alarm");
	}

	public async Task<AlarmLog> GetByWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true)
	{
		return await GetBy(filters, orderBy, withTracking, "Alarm");
	}

	public async Task<List<AlarmLog>> GetAllWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true)
	{
		return await GetAll(filters, orderBy, withTracking, includes: new[] { "Alarm" });
	}
}
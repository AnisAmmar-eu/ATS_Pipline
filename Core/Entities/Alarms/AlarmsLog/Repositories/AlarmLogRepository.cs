using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.Alarms.AlarmsLog.Repositories;

public class AlarmLogRepository : BaseEntityRepository<AnodeCTX, AlarmLog, DTOAlarmLog>, IAlarmLogRepository
{
	public AlarmLogRepository(AnodeCTX context) : base(context, [nameof(AlarmLog.Alarm)], [])
	{
	}

	public Task<AlarmLog> GetByIdWithIncludes(
		int id,
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		bool withTracking = true) => GetById(id, filters, withTracking, nameof(AlarmLog.Alarm));

	public Task<AlarmLog> GetByWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true) => GetByWithThrow(filters, orderBy, withTracking, nameof(AlarmLog.Alarm));

	public Task<List<AlarmLog>> GetAllWithIncludes(
		Expression<Func<AlarmLog, bool>>[]? filters = null,
		Func<IQueryable<AlarmLog>, IOrderedQueryable<AlarmLog>>? orderBy = null,
		bool withTracking = true) => GetAll(filters, orderBy, withTracking, includes: nameof(AlarmLog.Alarm));

	public Task<int> AckAlarmLogs(int[] idAlarmLogs)
	{
		DateTimeOffset now = DateTimeOffset.Now;
		return _context
			.AlarmLog
			.Where(alarmLog => idAlarmLogs.Contains(alarmLog.ID))
			.Where(alarmLog => !alarmLog.IsAck)
			.ExecuteUpdateAsync(s => s.SetProperty(
				alarmLog => alarmLog.IsAck,
				_ => true)
				.SetProperty(
					alarmLog => alarmLog.TSRead,
					_ => now));
	}
}
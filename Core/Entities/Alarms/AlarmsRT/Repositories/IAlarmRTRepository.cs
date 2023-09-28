using System.Linq.Expressions;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
namespace Core.Entities.Alarms.AlarmsRT.Repositories;

public interface IAlarmRTRepository : IRepositoryBaseEntity<AlarmRT, DTOAlarmRT>
{
	public Task<List<AlarmRT>> GetAllWithInclude(
		Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null,
		bool withTracking = true);
}
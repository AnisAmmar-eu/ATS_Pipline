using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarms.AlarmsRT.Repositories;

public class AlarmRTRepository : BaseEntityRepository<AnodeCTX, AlarmRT, DTOAlarmRT>, IAlarmRTRepository
{
	public AlarmRTRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<List<AlarmRT>> GetAllWithInclude(
		Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null,
		bool withTracking = true)
	{
		return await GetAll(filters, orderBy, withTracking, includes: nameof(AlarmRT.Alarm));
	}
}
using System.Linq.Expressions;
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.AlarmsRT.Models.DB;
using Core.Entities.AlarmsRT.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.AlarmsRT.Repositories;

public class AlarmRTRepository : RepositoryBaseEntity<AlarmCTX, AlarmRT, DTOAlarmRT>, IAlarmRTRepository
{
	public AlarmRTRepository(AlarmCTX context) : base(context)
	{
	}

	public async Task<List<AlarmRT>> GetAllWithInclude(
		Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null,
		bool withTracking = true)
	{
		return await GetAll(filters, orderBy, withTracking, includes: new[] { "Alarm" });
	}
}
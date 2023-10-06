using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using System.Linq.Expressions;
using Core.Shared.UnitOfWork;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public class AlarmRTService : ServiceBaseEntity<IAlarmRTRepository, AlarmRT, DTOAlarmRT>, IAlarmRTService
{
	public AlarmRTService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}

	public new async Task<List<DTOAlarmRT>> GetAll(Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null, bool withTracking = true,
		int? maxCount = null,
		params string[] includes)
	{
		return (await AlarmUOW.AlarmRT.GetAllWithInclude(filters, orderBy, withTracking)).ConvertAll(alarmRT =>
			alarmRT.ToDTO());
	}
}
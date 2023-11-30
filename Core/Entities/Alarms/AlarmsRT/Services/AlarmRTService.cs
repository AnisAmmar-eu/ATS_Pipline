using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public class AlarmRTService : BaseEntityService<IAlarmRTRepository, AlarmRT, DTOAlarmRT>, IAlarmRTService
{
	public AlarmRTService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	new public async Task<List<DTOAlarmRT>> GetAll(
		Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes)
	{
		return (await AnodeUOW.AlarmRT.GetAllWithInclude(filters, orderBy, withTracking)).ConvertAll(alarmRT =>
			alarmRT.ToDTO());
	}

	public async Task<int[]> GetAlarmRTStats()
	{
		List<AlarmRT> alarmRts = await AnodeUOW.AlarmRT.GetAll(withTracking: false);
		List<AlarmC> alarmCs = await AnodeUOW.AlarmC.GetAll(withTracking: false);
		int nbActiveAlarms = alarmRts.Count(alarmRT => alarmRT.IsActive);
		int nbNonAck = (int)alarmRts.Where(alarmRT => !alarmRT.IsActive).Sum(alarmRT => alarmRT.NbNonAck)!;
		int[] stats = new int[3];
		stats[0] = alarmCs.Count - nbActiveAlarms - nbNonAck;
		stats[1] = nbNonAck;
		stats[2] = nbActiveAlarms;
		return stats;
	}
}
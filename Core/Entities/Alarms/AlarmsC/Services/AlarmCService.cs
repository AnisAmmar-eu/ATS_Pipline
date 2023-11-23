using System.Linq.Expressions;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Services;

public class AlarmCService : ServiceBaseEntity<IAlarmCRepository, AlarmC, DTOAlarmC>, IAlarmCService
{
	public AlarmCService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	/// <summary>
	///     Search for an AlarmC based on its RID.
	/// </summary>
	/// <param name="rid"></param>
	/// <returns></returns>
	public async Task<DTOAlarmC> GetByRID(string rid)
	{
		return (await AnodeUOW.AlarmC.GetBy(new Expression<Func<AlarmC, bool>>[]
		{
			alarm => alarm.RID == rid
		})).ToDTO();
	}
}
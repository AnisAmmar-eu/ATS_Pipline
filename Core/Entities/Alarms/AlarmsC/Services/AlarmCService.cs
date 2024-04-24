using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Services;

public class AlarmCService : BaseEntityService<IAlarmCRepository, AlarmC, DTOAlarmC>, IAlarmCService
{
	public AlarmCService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	/// <summary>
	///     Search for an AlarmC based on its RID.
	/// </summary>
	/// <param name="rid"></param>
	public async Task<DTOAlarmC> GetByRID(string rid)
	{
		return (await _anodeUOW.AlarmC.GetByWithThrow([alarm => alarm.RID == rid])).ToDTO();
	}
}
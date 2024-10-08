using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Services;

public interface IAlarmCService : IBaseEntityService<AlarmC, DTOAlarmC>
{
	public Task<DTOAlarmC> GetByRID(string rid);
}
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Services;

public interface IAlarmCService : IServiceBaseEntity<AlarmC, DTOAlarmC>
{
	public Task<DTOAlarmC> GetByRID(string RID);
	public Task<DTOAlarmC> AddReceivedAlarmC(DTOAlarmC received);
}
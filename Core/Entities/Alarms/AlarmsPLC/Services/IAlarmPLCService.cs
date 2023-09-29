using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Shared.UnitOfWork;

namespace Core.Entities.Alarms.AlarmsPLC.Services;

public interface IAlarmPLCService
{
	public Task AddAlarmPLC(AlarmPLC alarmPLC);
}
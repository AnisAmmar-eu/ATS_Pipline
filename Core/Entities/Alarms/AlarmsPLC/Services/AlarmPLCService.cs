using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Services;

public class AlarmPLCService : ServiceBaseEntity<IAlarmPLCRepository, AlarmPLC, DTOAlarmPLC>, IAlarmPLCService
{
	public AlarmPLCService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}
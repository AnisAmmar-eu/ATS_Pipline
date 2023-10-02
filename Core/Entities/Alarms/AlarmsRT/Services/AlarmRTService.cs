using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Shared.SignalR;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Shared.Services.Kernel;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public class AlarmRTService : ServiceBaseEntity<IAlarmRTRepository, AlarmRT, DTOAlarmRT>, IAlarmRTService
{
	public AlarmRTService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}
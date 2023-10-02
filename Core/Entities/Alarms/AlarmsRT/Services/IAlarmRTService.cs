using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public interface IAlarmRTService : IServiceBaseEntity<AlarmRT, DTOAlarmRT>
{
}
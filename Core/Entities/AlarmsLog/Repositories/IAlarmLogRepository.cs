using Core.Entities.AlarmsLog.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Repositories;

public interface IAlarmLogRepository : IRepositoryBaseEntity<AlarmLog, DTOAlarmLog>
{
}
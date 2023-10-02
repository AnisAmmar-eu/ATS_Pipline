using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsCycle.Models.Repositories;

public interface IAlarmCycleRepository : IRepositoryBaseEntity<AlarmCycle, DTOAlarmCycle>
{
}
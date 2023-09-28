using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
namespace Core.Entities.Alarms.AlarmsCycle.Models.Repositories;

public interface IAlarmCycleRepository : IRepositoryBaseEntity<AlarmCycle, DTOAlarmCycle>
{
	
}
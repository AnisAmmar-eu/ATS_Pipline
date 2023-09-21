using Core.Entities.AlarmsCycle.Models.DB;
using Core.Entities.AlarmsCycle.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.AlarmsCycle.Models.Repositories;

public interface IAlarmCycleRepository : IRepositoryBaseEntity<AlarmCycle, DTOAlarmCycle>
{
	
}
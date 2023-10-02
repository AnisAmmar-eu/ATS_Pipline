using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarms.AlarmsCycle.Models.Repositories;

public class AlarmCycleRepository : RepositoryBaseEntity<AlarmCTX, AlarmCycle, DTOAlarmCycle>, IAlarmCycleRepository
{
	public AlarmCycleRepository(AlarmCTX context) : base(context)
	{
	}
}
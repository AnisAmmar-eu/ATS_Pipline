using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarms.AlarmsCycle.Models.Repositories;

public class AlarmCycleRepository : RepositoryBaseEntity<AnodeCTX, AlarmCycle, DTOAlarmCycle>, IAlarmCycleRepository
{
	public AlarmCycleRepository(AnodeCTX context) : base(context)
	{
	}
}
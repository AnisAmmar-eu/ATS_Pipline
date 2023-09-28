using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarms.AlarmsC.Repositories;

public class AlarmCRepository : RepositoryBaseEntity<AlarmCTX, AlarmC, DTOAlarmC>, IAlarmCRepository
{
	public AlarmCRepository(AlarmCTX context) : base(context)
	{
	}
}
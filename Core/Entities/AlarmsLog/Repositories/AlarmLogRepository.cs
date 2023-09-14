using Core.Entities.AlarmsLog.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;

namespace Core.Entities.AlarmsLog.Repositories;

public class AlarmLogRepository : RepositoryBaseEntity<AlarmCTX, AlarmLog, DTOAlarmLog>, IAlarmLogRepository
{
	public AlarmLogRepository(AlarmCTX context) : base(context)
	{
	}
}
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.AlarmsC.Repositories;

public class AlarmCRepository : RepositoryBaseEntity<AlarmCTX, AlarmC, DTOAlarmC>, IAlarmCRepository
{
    public AlarmCRepository(AlarmCTX context) : base(context)
    {
    }
}
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
namespace Core.Entities.Alarms.AlarmsPLC.Repositories;

public class AlarmPLCRepository : RepositoryBaseEntity<AlarmCTX, AlarmPLC, DTOAlarmPLC>, IAlarmPLCRepository
{
	public AlarmPLCRepository(AlarmCTX context) : base(context)
	{
	}
}
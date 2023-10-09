using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Alarms.AlarmsPLC.Repositories;

public class AlarmPLCRepository : RepositoryBaseEntity<AnodeCTX, AlarmPLC, DTOAlarmPLC>, IAlarmPLCRepository
{
	public AlarmPLCRepository(AnodeCTX context) : base(context)
	{
	}
}
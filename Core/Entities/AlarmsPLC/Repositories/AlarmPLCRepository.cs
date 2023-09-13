using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.AlarmsPLC.Repositories;

public class AlarmPLCRepository : RepositoryBaseEntity<AlarmCTX, AlarmPLC, DTOAlarmPLC>, IAlarmPLCRepository
{
	public AlarmPLCRepository(AlarmCTX context) : base(context)
	{
	}
}
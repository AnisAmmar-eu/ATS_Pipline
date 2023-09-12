
using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.AlarmesPLC.Models.DTO;
using Core.Entities.AlarmesPLC.Repositories;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

public class AlarmePLCRepository: RepositoryBaseEntity<AlarmesDbContext, AlarmePLC, DTOAlarmePLC>, IAlarmePLCRepository
{
    public AlarmePLCRepository(AlarmesDbContext context) : base(context)
    {
    }
}
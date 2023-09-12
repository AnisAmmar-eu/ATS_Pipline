using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.AlarmesPLC.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.AlarmesPLC.Repositories;

public interface IAlarmePLCRepository : IRepositoryBaseEntity<AlarmePLC, DTOAlarmePLC>
{
    
}
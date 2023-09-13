using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.AlarmsC.Repositories;

public interface IAlarmCRepository : IRepositoryBaseEntity<AlarmC, DTOAlarmC>
{
}
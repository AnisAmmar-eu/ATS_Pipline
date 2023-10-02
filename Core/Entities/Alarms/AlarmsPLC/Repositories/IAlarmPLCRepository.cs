using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsPLC.Repositories;

public interface IAlarmPLCRepository : IRepositoryBaseEntity<AlarmPLC, DTOAlarmPLC>
{
}
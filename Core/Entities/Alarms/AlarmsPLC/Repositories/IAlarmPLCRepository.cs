using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
namespace Core.Entities.Alarms.AlarmsPLC.Repositories;

public interface IAlarmPLCRepository : IRepositoryBaseEntity<AlarmPLC, DTOAlarmPLC>
{
}
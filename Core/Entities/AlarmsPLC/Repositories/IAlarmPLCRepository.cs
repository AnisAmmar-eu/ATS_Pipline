using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.AlarmsPLC.Repositories;

public interface IAlarmPLCRepository : IRepositoryBaseEntity<AlarmPLC, DTOAlarmPLC>
{
}
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Repositories;

public interface IAlarmCRepository : IBaseEntityRepository<AlarmC, DTOAlarmC>;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.UnitOfWork;

namespace Core.Entities.Alarms.AlarmsPLC.Services;

public interface IAlarmPLCService : IServiceBaseEntity<AlarmPLC, DTOAlarmPLC>
{
}
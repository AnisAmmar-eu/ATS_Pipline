using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Entities.ServicesMonitors.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.ServicesMonitors.Services;

public class ServicesMonitorService : ServiceBaseEntity<ServicesMonitorRepository, ServicesMonitor, DTOServicesMonitor>,
	IServicesMonitorService
{
	protected ServicesMonitorService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}
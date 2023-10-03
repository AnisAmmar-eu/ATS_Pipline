using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Services;

public interface IServicesMonitorService : IServiceBaseEntity<ServicesMonitor, DTOServicesMonitor>
{
	
}
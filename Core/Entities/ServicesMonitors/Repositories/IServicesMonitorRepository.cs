using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Repositories;

public interface IServicesMonitorRepository : IRepositoryBaseEntity<ServicesMonitor, DTOServicesMonitor>
{
	
}
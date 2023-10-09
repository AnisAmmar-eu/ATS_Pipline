using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.ServicesMonitors.Repositories;

public class ServicesMonitorRepository : RepositoryBaseEntity<AnodeCTX, ServicesMonitor, DTOServicesMonitor>,
	IServicesMonitorRepository
{
	public ServicesMonitorRepository(AnodeCTX context) : base(context)
	{
	}
}
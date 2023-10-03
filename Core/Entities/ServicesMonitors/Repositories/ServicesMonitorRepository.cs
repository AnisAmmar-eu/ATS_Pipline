using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.ServicesMonitors.Repositories;

public class ServicesMonitorRepository : RepositoryBaseEntity<AlarmCTX, ServicesMonitor, DTOServicesMonitor>,
	IServicesMonitorRepository
{
	public ServicesMonitorRepository(AlarmCTX context) : base(context)
	{
	}
}
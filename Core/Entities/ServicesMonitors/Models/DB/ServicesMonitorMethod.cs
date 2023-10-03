using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DB;

public partial class ServicesMonitor : BaseEntity, IBaseEntity<ServicesMonitor, DTOServicesMonitor>
{
	public DTOServicesMonitor ToDTO(string? languageRID = null)
	{
		return new DTOServicesMonitor(this);
	}
}
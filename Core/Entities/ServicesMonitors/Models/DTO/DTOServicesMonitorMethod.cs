using Core.Entities.ServicesMonitors.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DTO;

public partial class DTOServicesMonitor : DTOBaseEntity, IDTO<ServicesMonitor, DTOServicesMonitor>
{
	public DTOServicesMonitor(ServicesMonitor servicesMonitor)
	{
		ID = servicesMonitor.ID;
		TS = servicesMonitor.TS;
		RID = servicesMonitor.RID;
		Name = servicesMonitor.Name;
		Description = servicesMonitor.Description;
		IPAddress = servicesMonitor.IPAddress;
		IsConnected = servicesMonitor.IsConnected;
	}

	public override ServicesMonitor ToModel()
	{
		return new ServicesMonitor(this);
	}
}
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DB;

public partial class ServicesMonitor : BaseEntity, IBaseEntity<ServicesMonitor, DTOServicesMonitor>
{
	public ServicesMonitor()
	{
	}

	public ServicesMonitor(DTOServicesMonitor dtoServicesMonitor)
	{
		ID = dtoServicesMonitor.ID;
		TS = (DateTimeOffset)dtoServicesMonitor.TS!;
		RID = dtoServicesMonitor.RID;
		Name = dtoServicesMonitor.Name;
		Description = dtoServicesMonitor.Description;
		IPAddress = dtoServicesMonitor.IPAddress;
		IsConnected = dtoServicesMonitor.IsConnected;
	}

	public override DTOServicesMonitor ToDTO()
	{
		return new DTOServicesMonitor(this);
	}
}
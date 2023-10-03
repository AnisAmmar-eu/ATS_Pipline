using Core.Entities.ServicesMonitors.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DTO;

public partial class DTOServicesMonitor : DTOBaseEntity, IDTO<ServicesMonitor, DTOServicesMonitor>
{
	public string RID;
	public string Name;
	public string Description;
	public string IPAdress;
	public bool IsConnected;
}
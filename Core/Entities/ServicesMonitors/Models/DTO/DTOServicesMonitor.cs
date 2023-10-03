using Core.Entities.ServicesMonitors.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DTO;

public partial class DTOServicesMonitor : DTOBaseEntity, IDTO<ServicesMonitor, DTOServicesMonitor>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string IPAddress { get; set; }
	public bool IsConnected { get; set; }
}
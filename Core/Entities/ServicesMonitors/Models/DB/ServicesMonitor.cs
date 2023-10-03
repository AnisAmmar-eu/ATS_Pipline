using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DB;

public partial class ServicesMonitor : BaseEntity, IBaseEntity<ServicesMonitor, DTOServicesMonitor>
{
	public string RID;
	public string Name;
	public string Description;
	public string IPAdress;
	public bool IsConnected;

	public List<ExtTag> ExtTags = new List<ExtTag>();
}
using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ServicesMonitors.Models.DB;

public partial class ServicesMonitor : BaseEntity, IBaseEntity<ServicesMonitor, DTOServicesMonitor>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string IPAddress { get; set; }
	public bool IsConnected { get; set; }

	public List<ExtTag> ExtTags { get; set; } = new List<ExtTag>();
}
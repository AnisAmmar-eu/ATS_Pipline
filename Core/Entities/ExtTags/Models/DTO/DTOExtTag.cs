using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Models.DTO;

public partial class DTOExtTag : DTOBaseEntity, IDTO<ExtTag, DTOExtTag>
{
	public string CurrentValue;
	public string Description;
	public bool HasNewValue;

	public bool IsReadOnly;
	public string Name;
	public string NewValue;
	public string? Path;
	public string RID;
	public DTOServicesMonitor Service;
	public int ServiceID;

	public string ValueType;
}
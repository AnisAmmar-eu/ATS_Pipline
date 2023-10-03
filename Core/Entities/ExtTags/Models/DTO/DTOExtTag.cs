using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Models.DTO;

public partial class DTOExtTag : DTOBaseEntity, IDTO<ExtTag, DTOExtTag>
{
	public string RID;
	public string Name;
	public string Description;
	public int CurrentValue;
	public int NewValue;
	public bool IsReadOnly;
	public bool HasNewValue;
	public int ServiceID;
	public DTOServicesMonitor Service;
	public string? Path;
}
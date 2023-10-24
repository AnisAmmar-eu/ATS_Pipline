using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DTO;

public partial class DTOIOTTag : DTOBaseEntity, IDTO<IOTTag, DTOIOTTag>
{
	public string RID { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string CurrentValue { get; set; } = string.Empty;
	public string NewValue { get; set; } = string.Empty;
	public bool HasNewValue { get; set; }
	public string Path { get; set; } = string.Empty;
	public int IOTDeviceID { get; set; }
}
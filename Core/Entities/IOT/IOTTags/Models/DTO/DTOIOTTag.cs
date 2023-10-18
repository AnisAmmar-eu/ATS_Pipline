using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DTO;

public partial class DTOIOTTag : DTOBaseEntity, IDTO<IOTTag, DTOIOTTag>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string CurrentValue { get; set; }
	public string NewValue { get; set; }
	public bool HasNewValue { get; set; }
	public string Path { get; set; }
	public int IOTDeviceID { get; set; }
}
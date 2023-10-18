using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO;

public partial class DTOIOTDevice : DTOBaseEntity, IDTO<IOTDevice, DTOIOTDevice>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public bool IsConnected { get; set; }
	public List<DTOIOTTag> IOTTags { get; set; } = new();
}
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB;

public partial class IOTDevice : BaseEntity, IBaseEntity<IOTDevice, DTOIOTDevice>
{
	public string RID { get; set; }	
	public string Name { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public bool IsConnected { get; set; }

	#region Nav Properties

	public List<IOTTag> IOTTags { get; set; } = new();

	#endregion
}
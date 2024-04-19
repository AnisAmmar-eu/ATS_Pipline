using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB;

public partial class IOTDevice : BaseEntity, IBaseEntity<IOTDevice, DTOIOTDevice>
{
	public string RID { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string ConnectionPath { get; set; } = string.Empty;
	public bool IsConnected { get; set; }

	#region Nav Properties

	public List<IOTTag> IOTTags { get; set; } = [];

	#endregion Nav Properties
}
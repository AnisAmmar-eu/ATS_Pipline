using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DB;

public partial class IOTTag : BaseEntity, IBaseEntity<IOTTag, DTOIOTTag>
{
	public string RID { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string CurrentValue { get; set; }
	public string NewValue { get; set; }
	public bool HasNewValue { get; set; }
	public string Path { get; set; }
	public int IOTDeviceID { get; set; }

	#region Nav Properties

	private IOTDevice? _iotDevice;

	public IOTDevice IOTDevice
	{
		set => _iotDevice = value;
		get => _iotDevice
		       ?? throw new InvalidOperationException("Uninitialized property: " + nameof(IOTDevice));
	}

	#endregion
}
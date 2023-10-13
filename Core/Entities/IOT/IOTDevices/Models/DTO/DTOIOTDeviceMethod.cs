using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO;

public partial class DTOIOTDevice : DTOBaseEntity, IDTO<IOTDevice, DTOIOTDevice>
{
	public DTOIOTDevice()
	{
	}

	public DTOIOTDevice(IOTDevice iotDevice)
	{
		ID = iotDevice.ID;
		TS = iotDevice.TS;
		RID = iotDevice.RID;
		Name = iotDevice.Name;
		Description = iotDevice.Description;
		Address = iotDevice.Address;
		IsConnected = iotDevice.IsConnected;
		IOTTags = iotDevice.IOTTags.ConvertAll(tag => tag.ToDTO());
	}
}
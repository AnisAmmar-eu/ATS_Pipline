using Core.Entities.IOT.IOTDevices.Models.DB;

namespace Core.Entities.IOT.IOTDevices.Models.DTO;

public partial class DTOIOTDevice
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
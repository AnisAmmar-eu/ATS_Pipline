using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;

public partial class DTOOTCamera : DTOIOTDevice, IDTO<OTCamera, DTOOTCamera>
{
	public DTOOTCamera() : base()
	{
	}

	public DTOOTCamera(OTCamera otCamera) : base(otCamera)
	{
	}
}
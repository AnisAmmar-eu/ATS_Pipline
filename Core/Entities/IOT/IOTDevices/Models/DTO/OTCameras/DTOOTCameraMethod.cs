using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;

public partial class DTOOTCamera
{
	public DTOOTCamera()
	{
	}

	public DTOOTCamera(OTCamera otCamera) : base(otCamera)
	{
	}

	public override OTCamera ToModel() => new(this);
}
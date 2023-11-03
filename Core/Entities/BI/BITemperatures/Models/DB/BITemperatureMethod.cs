using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Models.DB;

public partial class BITemperature : BaseEntity, IBaseEntity<BITemperature, DTOBITemperature>
{
	public BITemperature()
	{
	}

	public BITemperature(OTCamera camera)
	{
		TS = DateTimeOffset.Now;
		CameraRID = camera.RID;
		StationID = Station.ID;
		Temperature = camera.Temperature;
	}

	public override DTOBITemperature ToDTO()
	{
		return new DTOBITemperature(this);
	}
}
using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Models.DTO;

public partial class DTOBITemperature : DTOBaseEntity, IDTO<BITemperature, DTOBITemperature>
{
	public DTOBITemperature()
	{
	}

	public DTOBITemperature(BITemperature biTemperature)
	{
		ID = biTemperature.ID;
		TS = biTemperature.TS;
		StationID = biTemperature.StationID;
		CameraRID = biTemperature.CameraRID;
		Temperature = biTemperature.Temperature;
	}
}
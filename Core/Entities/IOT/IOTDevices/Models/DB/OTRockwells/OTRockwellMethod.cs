using Core.Entities.IOT.IOTDevices.Models.DTO.OTRockwells;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTRockwells;

public partial class OTRockwell
{
	public override DTOOTRockwell ToDTO()
	{
		return new DTOOTRockwell(this);
	}
}
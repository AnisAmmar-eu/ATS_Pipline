using Core.Entities.IOT.IOTDevices.Models.DTO.OTTwinCats;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;

public partial class OTTwinCat : IOTDevice, IBaseEntity<OTTwinCat, DTOOTTwinCat>
{
	public override DTOOTTwinCat ToDTO()
	{
		return new DTOOTTwinCat(this);
	}
}
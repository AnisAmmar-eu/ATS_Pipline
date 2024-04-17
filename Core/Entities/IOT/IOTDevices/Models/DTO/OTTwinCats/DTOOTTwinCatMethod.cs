using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.OTTwinCats;

public partial class DTOOTTwinCat
{
	public DTOOTTwinCat()
	{
	}

	public DTOOTTwinCat(OTTwinCat otTwinCat) : base(otTwinCat)
	{
	}

	public override OTTwinCat ToModel() => new(this);
}
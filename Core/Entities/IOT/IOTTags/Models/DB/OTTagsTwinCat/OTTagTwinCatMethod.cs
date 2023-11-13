using Core.Entities.IOT.IOTTags.Models.DTO.OTTagsTwinCat;

namespace Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;

public partial class OTTagTwinCat
{
	public override DTOOTTagTwinCat ToDTO()
	{
		return new DTOOTTagTwinCat(this);
	}
}
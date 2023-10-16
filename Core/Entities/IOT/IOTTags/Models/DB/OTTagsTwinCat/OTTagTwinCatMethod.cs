using Core.Entities.IOT.IOTTags.Models.DTO.OTTagsTwinCat;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;

public partial class OTTagTwinCat : IOTTag, IBaseEntity<OTTagTwinCat, DTOOTTagTwinCat>
{
	public override DTOOTTagTwinCat ToDTO()
	{
		return new DTOOTTagTwinCat(this);
	}
}
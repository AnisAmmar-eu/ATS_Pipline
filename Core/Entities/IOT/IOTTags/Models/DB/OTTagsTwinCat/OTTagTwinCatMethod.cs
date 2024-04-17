using Core.Entities.IOT.IOTTags.Models.DTO.OTTagsTwinCat;

namespace Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;

public partial class OTTagTwinCat
{
	public OTTagTwinCat()
	{
	}

	public OTTagTwinCat(DTOOTTagTwinCat dtoOTTagTwinCat) : base(dtoOTTagTwinCat)
	{
	}

	public override DTOOTTagTwinCat ToDTO() => new(this);
}
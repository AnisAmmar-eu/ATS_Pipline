using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;

namespace Core.Entities.IOT.IOTTags.Models.DTO.OTTagsTwinCat;

public partial class DTOOTTagTwinCat
{
	public DTOOTTagTwinCat()
	{
	}

	public DTOOTTagTwinCat(OTTagTwinCat otTagTwinCat) : base(otTagTwinCat)
	{
		ValueType = otTagTwinCat.ValueType;
	}
}
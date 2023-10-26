using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DTO.OTTagsTwinCat;

public partial class DTOOTTagTwinCat : DTOIOTTag, IDTO<OTTagTwinCat, DTOOTTagTwinCat>
{
	public DTOOTTagTwinCat()
	{
	}

	public DTOOTTagTwinCat(OTTagTwinCat otTagTwinCat) : base(otTagTwinCat)
	{
		ValueType = otTagTwinCat.ValueType;
	}
}
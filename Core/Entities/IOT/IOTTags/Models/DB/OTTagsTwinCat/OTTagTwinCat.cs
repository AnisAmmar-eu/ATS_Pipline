using Core.Entities.IOT.IOTTags.Models.DTO.OTTagsTwinCat;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;

public partial class OTTagTwinCat : IOTTag, IBaseEntity<OTTagTwinCat, DTOOTTagTwinCat>
{
	public Type ValueType { get; set; } = typeof(int);
}
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DB;

public partial class IOTTag : BaseEntity, IBaseEntity<IOTTag, DTOIOTTag>
{
	public override DTOIOTTag ToDTO()
	{
		return new DTOIOTTag(this);
	}
}
using Core.Entities.ExtTags.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Models.DB;

public partial class ExtTag : BaseEntity, IBaseEntity<ExtTag, DTOExtTag>
{
	public DTOExtTag ToDTO(string? languageRID = null)
	{
		return new DTOExtTag(this);
	}
}
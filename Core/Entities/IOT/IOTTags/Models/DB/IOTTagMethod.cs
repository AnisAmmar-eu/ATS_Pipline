using Core.Entities.IOT.IOTTags.Models.DTO;

namespace Core.Entities.IOT.IOTTags.Models.DB;

public partial class IOTTag
{
	public override DTOIOTTag ToDTO()
	{
		return new DTOIOTTag(this);
	}
}
using Core.Entities.Vision.FileSettings.Models.DTO;

namespace Core.Entities.Vision.FileSettings.Models.DB;

public partial class FileSetting
{
	public override DTOFileSetting ToDTO()
	{
		return new DTOFileSetting(this);
	}
}
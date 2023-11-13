using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.FileSettings.Models.DB;

public partial class FileSetting : BaseEntity, IBaseEntity<FileSetting, DTOFileSetting>
{
	public override DTOFileSetting ToDTO()
	{
		return new DTOFileSetting(this);
	}
}
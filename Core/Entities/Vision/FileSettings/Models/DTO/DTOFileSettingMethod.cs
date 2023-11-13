using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.FileSettings.Models.DTO;

public partial class DTOFileSetting : DTOBaseEntity, IDTO<FileSetting, DTOFileSetting>
{
	public DTOFileSetting()
	{
	}

	public DTOFileSetting(FileSetting fileSetting)
	{
		ID = fileSetting.ID;
		TS = fileSetting.TS;
		RID = fileSetting.RID;
		FilePath = fileSetting.FilePath;
		LastModification = fileSetting.LastModification;
		LastUsername = fileSetting.LastUsername;
		LastUploadName = fileSetting.LastUploadName;
		LastComment = fileSetting.LastComment;
	}
}
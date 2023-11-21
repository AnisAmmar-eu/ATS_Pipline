using Core.Entities.Vision.FileSettings.Models.DTO;

namespace Core.Entities.Vision.FileSettings.Models.DB;

public partial class FileSetting
{
	public FileSetting()
	{
	}

	public FileSetting(DTOFileSetting dtoFileSetting) : base(dtoFileSetting)
	{
		RID = dtoFileSetting.RID;
		FilePath = dtoFileSetting.FilePath;
		LastModification = dtoFileSetting.LastModification;
		LastUsername = dtoFileSetting.LastUsername;
		LastUploadName = dtoFileSetting.LastUploadName;
		LastComment = dtoFileSetting.LastComment;
	}

	public override DTOFileSetting ToDTO()
	{
		return new DTOFileSetting(this);
	}
}
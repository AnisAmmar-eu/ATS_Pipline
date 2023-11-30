using Core.Entities.Vision.FileSettings.Models.DB;

namespace Core.Entities.Vision.FileSettings.Models.DTO;

public partial class DTOFileSetting
{
	public DTOFileSetting()
	{
	}

	public DTOFileSetting(FileSetting fileSetting) : base(fileSetting)
	{
		RID = fileSetting.RID;
		FilePath = fileSetting.FilePath;
		LastModification = fileSetting.LastModification;
		LastUsername = fileSetting.LastUsername;
		LastUploadName = fileSetting.LastUploadName;
		LastComment = fileSetting.LastComment;
	}

	public override FileSetting ToModel()
	{
		return new(this);
	}
}
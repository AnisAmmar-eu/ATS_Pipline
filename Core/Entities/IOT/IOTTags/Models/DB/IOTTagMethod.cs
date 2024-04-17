using Core.Entities.IOT.IOTTags.Models.DTO;

namespace Core.Entities.IOT.IOTTags.Models.DB;

public partial class IOTTag
{
	public IOTTag()
	{
	}

	public IOTTag(DTOIOTTag dtoIOTTag) : base(dtoIOTTag)
	{
		RID = dtoIOTTag.RID;
		Name = dtoIOTTag.Name;
		CurrentValue = dtoIOTTag.CurrentValue;
		NewValue = dtoIOTTag.NewValue;
		HasNewValue = dtoIOTTag.HasNewValue;
		IsReadOnly = dtoIOTTag.IsReadOnly;
	}

	public override DTOIOTTag ToDTO() => new(this);
}
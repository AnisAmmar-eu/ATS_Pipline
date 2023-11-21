using Core.Entities.IOT.IOTTags.Models.DB;

namespace Core.Entities.IOT.IOTTags.Models.DTO;

public partial class DTOIOTTag
{
	public DTOIOTTag()
	{
	}

	public DTOIOTTag(IOTTag iotTag) : base(iotTag)
	{
		RID = iotTag.RID;
		Name = iotTag.Name;
		CurrentValue = iotTag.CurrentValue;
		NewValue = iotTag.NewValue;
		HasNewValue = iotTag.HasNewValue;
		IsReadOnly = iotTag.IsReadOnly;
	}

	public override IOTTag ToModel()
	{
		return new IOTTag(this);
	}
}
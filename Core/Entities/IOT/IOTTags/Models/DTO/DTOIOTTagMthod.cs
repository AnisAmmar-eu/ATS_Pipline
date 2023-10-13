using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Models.DTO;

public partial class DTOIOTTag : DTOBaseEntity, IDTO<IOTTag, DTOIOTTag>
{
	public DTOIOTTag()
	{
	}

	public DTOIOTTag(IOTTag iotTag)
	{
		ID = iotTag.ID;
		TS = iotTag.TS;
		RID = iotTag.RID;
		Name = iotTag.Name;
		Description = iotTag.Description;
		CurrentValue = iotTag.CurrentValue;
		NewValue = iotTag.NewValue;
		HasNewValue = iotTag.HasNewValue;
		Path = iotTag.Path;
		IOTDeviceID = iotTag.IOTDeviceID;
	}
}
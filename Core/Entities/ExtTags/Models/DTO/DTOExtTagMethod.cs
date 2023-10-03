using Core.Entities.ExtTags.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Models.DTO;

public partial class DTOExtTag : DTOBaseEntity, IDTO<ExtTag, DTOExtTag>
{
	public DTOExtTag(ExtTag extTag)
	{
		ID = extTag.ID;
		TS = extTag.TS;
		RID = extTag.RID;
		Name = extTag.Name;
		Description = extTag.Description;
		ValueType = extTag.ValueType;
		CurrentValue = extTag.CurrentValue;
		NewValue = extTag.NewValue;
		IsReadOnly = extTag.IsReadOnly;
		HasNewValue = extTag.HasNewValue;
		ServiceID = extTag.ServiceID;
		Path = extTag.Path;
		Service = extTag.Service.ToDTO();
	}
}
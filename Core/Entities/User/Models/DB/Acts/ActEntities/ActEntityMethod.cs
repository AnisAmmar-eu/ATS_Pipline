using Core.Entities.User.Models.DTO.Acts.ActEntities;

namespace Core.Entities.User.Models.DB.Acts.ActEntities;

public partial class ActEntity
{
	public ActEntity()
	{
		RID = "";
	}

	public ActEntity(DTOActEntity dto)
	{
		RID = dto.RID == "" ? dto.Act?.RID + '.' + dto.EntityID : dto.RID;
		ParentID = dto.ParentID;
		EntityID = dto.EntityID;
	}

	public ActEntity(DTOActEntity dto, Act act, string signatureType)
	{
		RID = dto.RID == "" ? act.RID + '.' + dto.EntityID : dto.RID;
		ParentID = dto.ParentID;
		EntityID = dto.EntityID;
		SignatureType = signatureType;
		Act = act;
	}

	public override DTOActEntity ToDTO()
	{
		return new DTOActEntity(this);
	}
}
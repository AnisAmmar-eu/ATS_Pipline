using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.User.Models.DTO.Acts.ActEntities;

public partial class DTOActEntity : DTOBaseEntity, IDTO<ActEntity, DTOActEntity>
{
	public DTOActEntity()
	{
		RID = "";
	}

	public DTOActEntity(ActEntity actEntity)
	{
		ID = actEntity.ID;
		TS = actEntity.TS;
		Act = actEntity.Act.ToDTO();
		RID = actEntity.RID;
		EntityID = actEntity.EntityID;
		ParentID = actEntity.ParentID;
		SignatureType = actEntity.SignatureType;
		Applications = actEntity.ActEntityRoles
			.Select(aer => aer.ToDTO())
			.ToList();
	}

	public override ActEntity ToModel()
	{
		return new ActEntity(this);
	}

	public ActEntity ToModel(Act act, string signatureType)
	{
		return new ActEntity(this, act, signatureType);
	}

	public void AssociateEntity(string entityType, int entityID, string? parentType = null, int? parentID = null)
	{
		ParentID = parentID;
		EntityID = entityID;
		Act ??= new DTOAct();
		Act.ParentType = parentType;
		Act.EntityType = entityType;
	}
}